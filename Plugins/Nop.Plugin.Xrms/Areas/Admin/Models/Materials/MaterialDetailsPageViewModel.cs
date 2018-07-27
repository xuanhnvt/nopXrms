using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.Materials
{
    public partial class MaterialDetailsPageViewModel : CreateMaterialModel
    {
        public MaterialDetailsPageViewModel()
        {
            AvailableMaterialGroups = new List<SelectListItem>();
            AvailableWarehouses = new List<SelectListItem>();
        }

        public int Id { get; set; }

        // material groups
        public IList<SelectListItem> AvailableMaterialGroups { get; set; }

        // warehouse
        public IList<SelectListItem> AvailableWarehouses{ get; set; }

        //[NopResourceDisplayName("Xrms.Admin.Catalog.Materials.Fields.UsedQuantity")]
        //public int UsedQuantity { get; set; }

        public StockQuantityHistorySearchModel StockQuantityHistorySearch { get; set; }

        #region Nested classes

        #region Stock quantity history

        public partial class StockQuantityHistorySearchModel : BaseNopModel
        {
            //[NopResourceDisplayName("Xrms.Admin.Catalog.Materials.List.Search.MaterialName")]
            public int MaterialId { get; set; }

            [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.List.Search.Warehouse")]
            public int WarehouseId { get; set; }

        }

        public partial class StockQuantityHistoryListItemViewModel
        {

            public int Id { get; set; }

            [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.Warehouse")]
            public string WarehouseName { get; set; }

            [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.Combination")]
            public string AttributeCombination { get; set; }

            [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.QuantityAdjustment")]
            public int QuantityAdjustment { get; set; }

            [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.StockQuantity")]
            public int StockQuantity { get; set; }

            [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.Message")]
            public string Message { get; set; }

            [NopResourceDisplayName("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.CreatedOn")]
            [UIHint("DecimalNullable")]
            public DateTime CreatedOn { get; set; }
        }

        #endregion

        #endregion
    }
}
