using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Catalog;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Helpers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

using Nop.Plugin.Xrms.Areas.Admin.Models;
using Nop.Plugin.Xrms.Areas.Admin.Models.Tables;
using Nop.Plugin.Xrms.Services;
using Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.CurrentOrder;
using CQRSlite.Commands;
using Nop.Plugin.Xrms.Cqrs;
using System.Threading;

using Newtonsoft.Json;
using System.Threading.Tasks;
using Nop.Core.Domain.Logging;
using Nop.Services.Helpers;
using Nop.Plugin.Xrms.Hubs;
using Microsoft.AspNetCore.SignalR;
using Nop.Plugin.Xrms.Domain;
using Nop.Plugin.Xrms.Areas.Admin.Validators;
using FluentValidation.Results;
using Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders;
using Nop.Plugin.Xrms.Areas.Admin.Models.CashierOrders;

namespace Nop.Plugin.Xrms.Controllers
{
    public partial class CashierOrderController : BaseAdminController
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly ICommandSender _commandSender;
        private readonly ITableService _tableService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ICurrentOrderService _currentOrderService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly IWorkContext _workContext;
        private readonly IDateTimeHelper _dateTimeHelper;

        private readonly IClientNotificationService _clientNotificationService;

        #endregion Fields

        #region Ctor

        public CashierOrderController(ILogger logger,
            ICommandSender commandSender,
            ITableService tableService,
            ICurrentOrderService currentOrderService,
            IProductService productService,
            ICategoryService categoryService,
            IPictureService pictureService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ICustomerActivityService customerActivityService,
            IWorkContext workContext,
            IDateTimeHelper dateTimeHelper,
            IClientNotificationService clientNotificationService)
        {
            this._logger = logger;
            this._commandSender = commandSender;
            this._tableService = tableService;
            this._currentOrderService = currentOrderService;
            this._productService = productService;
            this._categoryService = categoryService;
            this._pictureService = pictureService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._customerActivityService = customerActivityService;
            this._workContext = workContext;
            this._dateTimeHelper = dateTimeHelper;
            this._clientNotificationService = clientNotificationService;
        }

        #endregion Ctor

        #region Utilities

        protected virtual void PrepareAvailableTables(CashierOrderDetailsPageViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var tables = _tableService.GetAllTables(showHidden: true);
            var list = tables.Where(result => result.State == TableState.Free || result.Id == model.OrderView.TableId).Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });
            foreach (var item in list)
                model.OrderView.AvailableTables.Add(item);
        }

        #endregion Utilities

        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedView();

            return View("~/Plugins/Xrms/Areas/Admin/Views/CashierOrder/List.cshtml");
        }

        [HttpPost]
        public virtual IActionResult List(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedKendoGridJson();

            var orders = _currentOrderService.GetAllOrders(command.Page - 1, command.PageSize);

            // prepare list view model
            var listViewModel = new DataSourceResult
            {
                Data = orders.Select(x =>
                {
                    var rowViewModel = x.ToListItemViewModel();
                    rowViewModel.CreatedOnUtc = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc).ToString("dd/MM HH:mm:ss");
                    rowViewModel.UpdatedOnUtc = _dateTimeHelper.ConvertToUserTime(x.UpdatedOnUtc, DateTimeKind.Utc).ToString("dd/MM HH:mm:ss");
                    if (x.BilledOnUtc.HasValue)
                    {
                        rowViewModel.BilledOnUtc = _dateTimeHelper.ConvertToUserTime((DateTime)x.BilledOnUtc.Value, DateTimeKind.Utc).ToString("dd/MM HH:mm:ss");
                    }
                    return rowViewModel;
                }),
                Total = orders.TotalCount
            };
            return Json(listViewModel);
        }

        #endregion List

        #region Create / Edit / Delete

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedView();

            var viewModel = new CashierOrderDetailsPageViewModel();

            // prepare view model
            PrepareAvailableTables(viewModel);

            return View("~/Plugins/Xrms/Areas/Admin/Views/CashierOrder/Create.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string jsonModel, CancellationToken cancellationToken)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedView();

            if (!String.IsNullOrEmpty(jsonModel))
            {
                try
                {
                    CreateInStoreOrderModel model = JsonConvert.DeserializeObject<CreateInStoreOrderModel>(jsonModel);

                    // validate model
                    CreateInStoreOrderModelValidator validator = new CreateInStoreOrderModelValidator(this._localizationService);
                    ValidationResult results = validator.Validate(model);

                    if (!results.IsValid)
                    {
                        _logger.InsertLog(LogLevel.Error, "Create Order: Validation Error", results.ToString());
                        List<string> errorList = new List<string>();
                        foreach (var failure in results.Errors)
                        {
                            //Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                            errorList.Add(failure.ErrorMessage);
                        }
                        return BadRequest(errorList);
                    }

                    // no error, send command
                    Guid guid = CompGuid.NewGuid();
                    await _commandSender.Send(new CreateCmd(guid, model), cancellationToken);
                    return Ok(guid);
                }
                catch (Exception ex)
                {
                    _logger.InsertLog(LogLevel.Error, "Create Order: Exception Error", ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            else
            {
                _logger.InsertLog(LogLevel.Error, "Create Order: Empty Object", "Request object is null or empty.");
                return BadRequest("Request object is null or empty.");
            }
        }

        public virtual IActionResult Edit(Guid id)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedView();

            var order = _currentOrderService.GetOrderByGuid(id);
            if (order == null)
                // no order found with the specified id
                return RedirectToAction("List");

            var viewModel = new CashierOrderDetailsPageViewModel();
            // prepare view model
            viewModel.OrderView.AggregateId = order.AggregateId;
            viewModel.OrderView.Version = order.Version;
            viewModel.OrderView.Id = order.Id;
            viewModel.OrderView.TableId = order.TableId;
            viewModel.OrderView.TableName = order.Table.Name;
            PrepareAvailableTables(viewModel);

            return View("~/Plugins/Xrms/Areas/Admin/Views/CashierOrder/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public virtual IActionResult GetOrderItems(int orderId)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedKendoGridJson();

            var orderItems = _currentOrderService.GetOrderItems(orderId);

            // prepare list view model
            var listViewModel = new DataSourceResult
            {
                Data = orderItems.Select(item =>
                {
                    var product = _productService.GetProductById(item.ProductId);
                    // fill in model values from the entity
                    var rowViewModel = new InStoreOrderItemListRowViewModel()
                    {
                        Id = item.Id,
                        AggregateId = item.AggregateId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        OldQuantity = item.Quantity,
                        Version = item.Version,

                        ProductName = product.Name,
                        ProductPrice = product.Price
                    };
                    return rowViewModel;
                }),
                Total = orderItems.Count
            };
            return Json(listViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderItems(string hubConnectionId, string jsonModel, CancellationToken cancellationToken)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedView();

            if (!String.IsNullOrEmpty(jsonModel))
            {
                try
                {
                    UpdateInStoreOrderItemsModel model = JsonConvert.DeserializeObject<UpdateInStoreOrderItemsModel>(jsonModel);

                    _logger.InsertLog(LogLevel.Information, "XRMS current in-store order info", String.Format("AggregateId = {0}, Version = {1}", model.AggregateId, model.Version));

                    // validate model

                    // save hub connection into session
                    HttpContext.Session.SetString("HubConnectionId", hubConnectionId);

                    // send command
                    await _commandSender.Send(new UpdateOrderItemsCmd(model.AggregateId, model.Version, model), cancellationToken);

                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.InsertLog(LogLevel.Error, "Update Order Items: Exception Error", ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            else
            {
                _logger.InsertLog(LogLevel.Error, "Update Order Items: Empty Object", "Request object is null or empty.");
                return BadRequest("Request object is null or empty.");
            }
        }

        #endregion Create / Edit / Delete

        #region Product List

        [HttpPost]
        public virtual IActionResult SearchProducts(DataSourceRequest command, ProductListViewModel.SearchProductsModel searchModel)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedKendoGridJson();

            var categoryIds = new List<int> { searchModel.CategoryId };

            // get products
            var products = _productService.SearchProducts(showHidden: true,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                categoryIds: categoryIds,
                keywords: searchModel.ProductName);

            // prepare list view model
            var listViewModel = new DataSourceResult
            {
                Data = products.Select(product =>
                {
                    // fill in model values from the entity
                    var rowViewModel = new ProductListViewModel.ProductListRowViewModel()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = 1
                    };
                    return rowViewModel;
                }),
                Total = products.TotalCount
            };
            return Json(listViewModel);
        }
        #endregion
    }
}