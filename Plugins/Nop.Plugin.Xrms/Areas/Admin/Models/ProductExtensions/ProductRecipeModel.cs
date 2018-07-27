using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.ProductExtensions
{

    /// <summary>
    /// Represents a product recipe model
    /// </summary>
    public partial class ProductRecipeModel : BaseNopEntityModel
    {
        #region Properties

        public int MaterialId { get; set; }

        public int ProductId { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Catalog.Categories.Products.Fields.Product")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Catalog.Categories.Products.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Catalog.Categories.Products.Fields.Unit")]
        public string Unit { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Catalog.Categories.Products.Fields.Quantity")]
        public int Quantity { get; set; }

        #endregion
    }
}
