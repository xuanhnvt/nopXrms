using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.Materials
{
    public partial class SearchMaterialsModel : BaseNopModel
    {

        public SearchMaterialsModel()
        {

        }

        [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.List.Search.MaterialName")]
        public string SearchMaterialName { get; set; }
        [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.List.Search.MaterialGroup")]
        public int SearchMaterialGroupId { get; set; }
        [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.List.Search.IncludeSubGroup")]
        public bool SearchIncludeSubGroup { get; set; }
        [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.List.Search.Supplier")]
        public int SearchSupplierId { get; set; }
        [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.List.Search.Warehouse")]
        public int SearchWarehouseId { get; set; }
    }
}
