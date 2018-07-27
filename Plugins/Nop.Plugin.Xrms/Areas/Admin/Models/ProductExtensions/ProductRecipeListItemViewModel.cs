using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.ProductExtensions
{
    public partial class ProductRecipeListItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MaterialId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public int DisplayOrder { get; set; }
    }
}
