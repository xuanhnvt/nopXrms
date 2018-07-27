using System;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.Suppliers
{
    public partial class SearchSuppliersModel : BaseNopModel
    {
        [NopResourceDisplayName("Xrms.Admin.Catalog.Suppliers.List.Search.SupplierName")]
        public string SearchSupplierName { get; set; }
    }
}
