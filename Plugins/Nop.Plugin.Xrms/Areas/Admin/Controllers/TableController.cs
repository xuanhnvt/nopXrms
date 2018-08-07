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

namespace Nop.Plugin.Xrms.Controllers
{
    public partial class TableController : BaseAdminController
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IExportManager _exportManager;
        private readonly IImportManager _importManager;
        private readonly ILocalizationService _localizationService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ITableService _materialGroupService;
        private readonly IMaterialService _materialService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly IShippingService _shippingService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;

        #endregion Fields

        #region Ctor

        public TableController(ITableService materialGroupService,
            IMaterialService materialService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IPictureService pictureService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IExportManager exportManager,
            ICustomerActivityService customerActivityService,
            IWorkContext workContext,
            IShippingService shippingService,
            IImportManager importManager,
            IStaticCacheManager cacheManager)
        {
            this._materialGroupService = materialGroupService;
            this._materialService = materialService;

            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._pictureService = pictureService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._exportManager = exportManager;
            this._customerActivityService = customerActivityService;
            this._workContext = workContext;
            this._importManager = importManager;
            this._cacheManager = cacheManager;
            this._shippingService = shippingService;
        }

        #endregion Ctor

        #region Utilities

        #endregion Utilities

        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageTables))
                return AccessDeniedView();

            var model = new TableListPageViewModel();

            return View("~/Plugins/Xrms/Areas/Admin/Views/Table/List.cshtml", model);
        }

        [HttpPost]
        public virtual IActionResult List(DataSourceRequest command, SearchTablesModel model)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageTables))
                return AccessDeniedKendoGridJson();

            var groups = _materialGroupService.GetAllTables(model.SearchTableName,
                command.Page - 1, command.PageSize, true);
            var gridModel = new DataSourceResult
            {
                Data = groups.Select(x =>
                {
                    var groupModel = x.ToListItemViewModel();
                    return groupModel;
                }),
                Total = groups.TotalCount
            };
            return Json(gridModel);
        }

        #endregion List

        #region Create / Edit / Delete

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageTables))
                return AccessDeniedView();

            var model = new TableDetailsPageViewModel();
            // set default values
            model.DisplayOrder = 1;

            return View("~/Plugins/Xrms/Areas/Admin/Views/Table/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(CreateTableModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageTables))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var group = model.ToEntity();
                group.CreatedOnUtc = DateTime.UtcNow;
                group.UpdatedOnUtc = DateTime.UtcNow;
                _materialGroupService.InsertTable(group);

                //activity log
                _customerActivityService.InsertActivity("AddNewTable", _localizationService.GetResource("Xrms.ActivityLog.AddNewTable"), group);

                SuccessNotification(_localizationService.GetResource("Xrms.Admin.Catalog.Tables.Notifications.Created"));

                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit", new { id = group.Id });
                }
                return RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            var viewModel = new TableDetailsPageViewModel();
            model.ToDetailsViewModel(viewModel);
            //viewModel.TableInfo = model;

            return View("~/Plugins/Xrms/Areas/Admin/Views/Table/Create.cshtml", viewModel);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageTables))
                return AccessDeniedView();

            var materialGroup = _materialGroupService.GetTableById(id);
            if (materialGroup == null || materialGroup.Deleted)
                //No group found with the specified id
                return RedirectToAction("List");

            var viewModel = materialGroup.ToDetailsViewModel();

            return View("~/Plugins/Xrms/Areas/Admin/Views/Table/Edit.cshtml", viewModel);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(int id, UpdateTableModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageTables))
                return AccessDeniedView();

            var materialGroup = _materialGroupService.GetTableById(id);
            if (materialGroup == null || materialGroup.Deleted)
                //No group found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var prevPictureId = materialGroup.PictureId;
                materialGroup = model.ToEntity(materialGroup);
                materialGroup.UpdatedOnUtc = DateTime.UtcNow;
                _materialGroupService.UpdateTable(materialGroup);

                //delete an old picture (if deleted or updated)
                if (prevPictureId > 0 && prevPictureId != materialGroup.PictureId)
                {
                    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }

                //activity log
                _customerActivityService.InsertActivity("EditTable", _localizationService.GetResource("Xrms.ActivityLog.EditTable"), materialGroup);

                SuccessNotification(_localizationService.GetResource("Xrms.Admin.Catalog.Tables.Notifications.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit", new { id = materialGroup.Id });
                }
                return RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            var viewModel = new TableDetailsPageViewModel();
            model.ToDetailsViewModel(viewModel);
            //viewModel.TableInfo = model;

            return View("~/Plugins/Xrms/Areas/Admin/Views/Table/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(XrmsPermissionProvider.ManageTables))
                return AccessDeniedView();

            var materialGroup = _materialGroupService.GetTableById(id);
            if (materialGroup == null)
                // No group found with the specified id
                return RedirectToAction("List");

            _materialGroupService.DeleteTable(materialGroup);

            //activity log
            _customerActivityService.InsertActivity("DeleteTable", _localizationService.GetResource("Xrms.ActivityLog.DeleteTable"), materialGroup);

            SuccessNotification(_localizationService.GetResource("Xrms.Admin.Catalog.Tables.Notifications.Deleted"));
            return RedirectToAction("List");
        }

        #endregion Create / Edit / Delete

        #region Export / Import

        public virtual IActionResult ExportXml()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            try
            {
                var xml = _exportManager.ExportCategoriesToXml();
                return File(Encoding.UTF8.GetBytes(xml), "application/xml", "categories.xml");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        public virtual IActionResult ExportXlsx()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            try
            {
                var bytes = _exportManager.ExportCategoriesToXlsx(_categoryService.GetAllCategories(showHidden: true).Where(p => !p.Deleted).ToList());
                return File(bytes, MimeTypes.TextXlsx, "categories.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual IActionResult ImportFromXlsx(IFormFile importexcelfile)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            //a vendor cannot import categories
            if (_workContext.CurrentVendor != null)
                return AccessDeniedView();

            try
            {
                if (importexcelfile != null && importexcelfile.Length > 0)
                {
                    _importManager.ImportCategoriesFromXlsx(importexcelfile.OpenReadStream());
                }
                else
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
                    return RedirectToAction("List");
                }
                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Imported"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        #endregion Export / Import

    }
}