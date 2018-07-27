using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.ProductExtensions
{
    public partial class AddProductRecipesPopupViewModel
    {
        public AddProductRecipesPopupViewModel()
        {
            AvailableMaterialGroups = new List<SelectListItem>();
            AvailableSuppliers = new List<SelectListItem>();
            AvailableWarehouses = new List<SelectListItem>();
        }

        public IList<SelectListItem> AvailableMaterialGroups { get; set; }
        public IList<SelectListItem> AvailableSuppliers { get; set; }
        public IList<SelectListItem> AvailableWarehouses { get; set; }

        public SearchMaterialsModel SearchModel { get; set; }

        #region Nested classes

        public partial class SearchMaterialsModel : BaseNopModel
        {
            [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.List.Search.MaterialName")]
            public string SearchMaterialName { get; set; }
            [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.List.Search.MaterialGroup")]
            public int SearchMaterialGroupId { get; set; }
            [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.List.Search.IncludeSubGroup")]
            public bool SearchIncludeSubGroup { get; set; }
            [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.List.Search.Warehouse")]
            public int SearchWarehouseId { get; set; }
        }

        public partial class AddMaterialsPopupModel : BaseNopModel
        {
            public int ProductId { get; set; }
            public int[] SelectedMaterialIds { get; set; }
        }

        #endregion
    }

    
}
