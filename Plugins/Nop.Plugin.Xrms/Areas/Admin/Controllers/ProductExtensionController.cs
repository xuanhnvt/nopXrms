using System;
using System.Collections.Generic;
using System.Linq;
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

using Nop.Plugin.Xrms.Areas.Admin.Models.MaterialGroups;
using Nop.Plugin.Xrms.Areas.Admin.Models.ProductExtensions;
using Nop.Plugin.Xrms.Domain;
using Nop.Plugin.Xrms.Services;

namespace Nop.Plugin.Xrms.Controllers
{
    public partial class ProductExtensionController : BaseAdminController
    {
        #region Fields

        private readonly IMaterialGroupService _materialGroupService;
        private readonly IMaterialService _materialService;

        private readonly IProductService _productService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IExportManager _exportManager;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IWorkContext _workContext;
        private readonly IImportManager _importManager;
        private readonly IShippingService _shippingService;
        private readonly IStaticCacheManager _cacheManager;
        
        #endregion
        
        #region Ctor

        public ProductExtensionController(IMaterialGroupService materialGroupService,
            IMaterialService materialService,
            IProductService productService,
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

            this._productService = productService;
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
        
        #endregion
        
        #region Utilities

        protected virtual void PrepareAvailableMaterialGroups(MaterialGroupDetailsPageViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.AvailableMaterialGroups.Add(new SelectListItem
            {
                Text = _localizationService.GetResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Parent.None"),
                Value = "0"
            });
            var groups = _materialGroupService.GetAllMaterialGroups(showHidden: true);
            var list = groups.Select(c => new SelectListItem
            {
                Text = c.GetFormattedBreadCrumb(_materialGroupService),
                Value = c.Id.ToString()
            });
            foreach (var item in list)
                model.AvailableMaterialGroups.Add(item);
        }


        protected virtual List<int> GetChildGroupIds(int parentGroupId)
        {
            var groupsIds = new List<int>();
            var groups = _materialGroupService.GetMaterialGroupsByParentGroupId(parentGroupId, true);
            foreach (var group in groups)
            {
                groupsIds.Add(group.Id);
                groupsIds.AddRange(GetChildGroupIds(group.Id));
            }
            return groupsIds;
        }

        #endregion
        
        #region Materials

        [HttpPost]
        public virtual IActionResult ProductRecipes(DataSourceRequest command, int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedKendoGridJson();

            //try to get a product with the specified id
            var product = _productService.GetProductById(productId)
                ?? throw new ArgumentException("No product found with the specified id");

            var materials = _materialService.GetProductRecipesByProductId(productId, true);
            var gridModel = new DataSourceResult
            {
                Data = materials.Select(x => new ProductRecipeListItemViewModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    MaterialId = x.MaterialId,
                    Name = x.Material.Name,
                    Unit = x.Unit,
                    Quantity = x.Quantity,
                    DisplayOrder = x.DisplayOrder
                }),
                Total = materials.Count
            };
            return Json(gridModel);
        }

        public virtual IActionResult DeleteProductRecipe(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //try to get a product recipe with the specified id
            var productRecipe = _materialService.GetProductRecipeById(id)
                ?? throw new ArgumentException("No product recipe found with the specified id", nameof(id));

            _materialService.DeleteProductRecipeItem(productRecipe);

            return new NullJsonResult();
        }
        
        public virtual IActionResult UpdateProductRecipe(ProductRecipeModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //try to get a product recipe with the specified id
            var productRecipe = _materialService.GetProductRecipeById(model.Id)
                ?? throw new ArgumentException("No product recipe found with the specified id");

            productRecipe.Quantity = model.Quantity;
            productRecipe.DisplayOrder = model.DisplayOrder;
            _materialService.UpdateProductRecipe(productRecipe);

            return new NullJsonResult();
        }

        public virtual IActionResult AddProductRecipesPopup(int materialGroupId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();
            
            var model = new AddProductRecipesPopupViewModel();

            // groups
            model.AvailableMaterialGroups.Add(new SelectListItem
            {
                Text = _localizationService.GetResource("Admin.Common.All"),
                Value = "0"
            });
            var groups = _materialGroupService.GetAllMaterialGroups(showHidden: true);

            var list = groups.Select(c => new SelectListItem
            {
                Text = c.GetFormattedBreadCrumb(_materialGroupService),
                Value = c.Id.ToString()
            });
            foreach (var item in list)
                model.AvailableMaterialGroups.Add(item);

            //manufacturers
            model.AvailableSuppliers.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            var manufacturers = SelectListHelper.GetManufacturerList(_manufacturerService, _cacheManager, true);
            foreach (var m in manufacturers)
                model.AvailableSuppliers.Add(m);

            //warehouses
            model.AvailableWarehouses.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var wh in _shippingService.GetAllWarehouses())
                model.AvailableWarehouses.Add(new SelectListItem { Text = wh.Name, Value = wh.Id.ToString() });

            //return View(model);
            return View("~/Plugins/Xrms/Areas/Admin/Views/ProductExtension/AddProductRecipesPopup.cshtml", model);
        }

        [HttpPost]
        public virtual IActionResult SearchMaterials(DataSourceRequest command, AddProductRecipesPopupViewModel.SearchMaterialsModel model)
        {
                if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                    return AccessDeniedKendoGridJson();

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var groupIds = new List<int> { model.SearchMaterialGroupId };
                // include sub groups
                if (model.SearchIncludeSubGroup && model.SearchMaterialGroupId > 0)
                    groupIds.AddRange(GetChildGroupIds(model.SearchMaterialGroupId));

                var materials = _materialService.SearchMaterials(
                    groupIds: groupIds,
                    warehouseId: model.SearchWarehouseId,
                    keywords: model.SearchMaterialName,
                    pageIndex: command.Page - 1,
                    pageSize: command.PageSize,
                    showHidden: true
                );
                var gridModel = new DataSourceResult
                {
                    Data = materials.Select(x =>
                    {
                        var materialModel = new
                        {
                            Id = x.Id,
                            PictureThumbnailUrl = _pictureService.GetPictureUrl(x.PictureId, 75, true),
                            Name = x.Name,
                            Unit = x.Unit,
                            Group = _materialGroupService.GetMaterialGroupById(x.MaterialGroupId).GetFormattedBreadCrumb(_materialGroupService)
                        };

                        return materialModel;
                    }),
                    Total = materials.TotalCount
                };

                return Json(gridModel);
            }
        
        [HttpPost]
        public virtual IActionResult AddProductRecipesPopup(AddProductRecipesPopupViewModel.AddMaterialsPopupModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //get selected materials
            var selectedMaterials = _materialService.GetMaterialsByIds(model.SelectedMaterialIds.ToArray());
            if (selectedMaterials.Any())
            {
                var existingProductRecipes = _materialService.GetProductRecipesByProductId(model.ProductId, showHidden: true);
                foreach (var material in selectedMaterials)
                {
                    foreach (var productRecipe in existingProductRecipes)
                        if (productRecipe.ProductId == model.ProductId && productRecipe.MaterialId == material.Id)
                            continue;

                    //insert the new product recipe
                    _materialService.InsertProductRecipe(new ProductRecipe
                    {
                        ProductId = model.ProductId,
                        MaterialId = material.Id,
                        Quantity = 0,
                        DisplayOrder = 1
                    });
                }
            }

            return Ok();
        }

        #endregion
    }
}