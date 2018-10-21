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
using Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders;
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

        protected virtual void PrepareAvailableTables(CurrentOrderDetailsPageViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var tables = _tableService.GetAllTables(showHidden: true);
            var list = tables.Where(result => result.State == TableState.Free || result.Id == model.TableId).Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });
            foreach (var item in list)
                model.AvailableTables.Add(item);
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

            var viewModel = new CurrentOrderListPageViewModel();

            return View("~/Plugins/Xrms/Areas/Admin/Views/CashierOrder/List.cshtml", viewModel);
        }

        [HttpPost]
        public virtual IActionResult List(DataSourceRequest command, SearchCurrentOrdersModel model)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedKendoGridJson();

            var orders = _currentOrderService.GetAllOrders(command.Page - 1, command.PageSize);

            // prepare list view model
            var gridViewModel = new DataSourceResult
            {
                Data = orders.Select(x =>
                {
                    var itemViewModel = x.ToListItemViewModel();
                    itemViewModel.CreatedOnUtc = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc).ToString("dd/MM HH:mm:ss");
                    itemViewModel.UpdatedOnUtc = _dateTimeHelper.ConvertToUserTime(x.UpdatedOnUtc, DateTimeKind.Utc).ToString("dd/MM HH:mm:ss");
                    if (x.BilledOnUtc.HasValue)
                    {
                        itemViewModel.BilledOnUtc = _dateTimeHelper.ConvertToUserTime((DateTime)x.BilledOnUtc.Value, DateTimeKind.Utc).ToString("dd/MM HH:mm:ss");
                    }
                    return itemViewModel;
                }),
                Total = orders.TotalCount
            };
            return Json(gridViewModel);
        }

        #endregion List

        #region Create / Edit / Delete

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedView();

            var viewModel = new CurrentOrderDetailsPageViewModel();
            // prepare view model
            PrepareAvailableTables(viewModel);

            return View("~/Plugins/Xrms/Areas/Admin/Views/CashierOrder/Create.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string model, CancellationToken cancellationToken)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedView();

            if (!String.IsNullOrEmpty(model))
            {
                try
                {
                    CreateCurrentOrderModel orderModel = JsonConvert.DeserializeObject<CreateCurrentOrderModel>(model);
                    Guid guid = CompGuid.NewGuid();
                    int length = orderModel.AddedOrderItems.Count;
                    await _commandSender.Send(new CreateCmd(guid, orderModel), cancellationToken);
                    return Json(guid);
                }
                catch (Exception ex)
                {
                    _logger.InsertLog(LogLevel.Error, "Create order error", ex.Message);
                    return Json("An Error Has occoured");
                }
            }
            else
            {
                return Json("An Error Has occoured");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderItems(string hubConnectionId, string model, CancellationToken cancellationToken)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedView();

            if (!String.IsNullOrEmpty(model))
            {
                try
                {
                    UpdateCurrentOrderItemsModel orderModel = JsonConvert.DeserializeObject<UpdateCurrentOrderItemsModel>(model);

                    _logger.InsertLog(LogLevel.Information, "XRms order info", String.Format("AggregateId = {0}, Version = {1}", orderModel.AggregateId, orderModel.Version));
                    //session save
                    HttpContext.Session.SetString("HubConnectionId", hubConnectionId);

                    await _commandSender.Send(new UpdateOrderItemsCmd(orderModel.AggregateId, orderModel.Version, orderModel), cancellationToken);
                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.InsertLog(LogLevel.Error, "Update order items error", ex.Message);
                    throw ex;
                    //return Json("An Error Has occoured");
                }
            }
            else
            {
                return Json("An Error Has occoured");
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

            var viewModel = new CurrentOrderDetailsPageViewModel();
            // set value
            viewModel.AggregateId = order.AggregateId;
            viewModel.Version = order.Version;
            viewModel.Id = order.Id;
            viewModel.TableId = order.TableId;
            PrepareAvailableTables(viewModel);

            return View("~/Plugins/Xrms/Areas/Admin/Views/CashierOrder/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public virtual IActionResult GetOrderItems(int currentOrderId)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedKendoGridJson();

            var orderItems = _currentOrderService.GetOrderItems(currentOrderId);

            // prepare list view model
            var gridModel = new DataSourceResult
            {
                Data = orderItems.Select(item =>
                {
                    var product = _productService.GetProductById(item.ProductId);
                    // fill in model values from the entity
                    var orderItemModel = new CurrentOrderDetailsPageViewModel.OrderItemViewModel()
                    {
                        Id = item.Id,
                        AggregateId = item.AggregateId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        OldQuantity = item.Quantity,
                        //CurrentOrderId = item.CurrentOrderId,
                        Version = item.Version,

                        ProductName = product.Name,
                        ProductPrice = product.Price
                    };
                    return orderItemModel;
                }),
                Total = orderItems.Count
            };
            return Json(gridModel);
        }
        #endregion Create / Edit / Delete

        #region Product List

        [HttpPost]
        public virtual IActionResult SearchProducts(DataSourceRequest command, CurrentOrderDetailsPageViewModel.SearchProductsModel searchModel)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageCashierOrders))
                return AccessDeniedKendoGridJson();

            var categoryIds = new List<int> { searchModel.SearchCategoryId };

            // get products
            var products = _productService.SearchProducts(showHidden: true,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                categoryIds: categoryIds,
                keywords: searchModel.SearchProductName);

            // prepare list view model
            var gridModel = new DataSourceResult
            {
                Data = products.Select(product =>
                {
                    // fill in model values from the entity
                    var productModel = new CurrentOrderDetailsPageViewModel.ProductListItemViewModel()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = 1
                    };
                    return productModel;
                }),
                Total = products.TotalCount
            };
            return Json(gridModel);
        }

        #endregion
    }
}