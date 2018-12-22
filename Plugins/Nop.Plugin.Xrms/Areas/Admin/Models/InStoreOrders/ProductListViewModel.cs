using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders
{
    public partial class ProductListViewModel
    {
        public ProductListViewModel()
        {
            SearchProductsView = new SearchProductsViewModel();
        }
        public SearchProductsViewModel SearchProductsView { get; set; }

        #region Nested classes

        public partial class ProductListRowViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }

        public partial class SearchProductsViewModel : SearchProductsModel
        {
            public SearchProductsViewModel()
            {
                AvailableCategories = new List<SelectListItem>();
            }
            public IList<SelectListItem> AvailableCategories { get; set; }
        }

        public partial class SearchProductsModel
        {
            [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Details.ProductList.Search.ProductName")]
            public string ProductName { get; set; }
            [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Details.ProductList.Search.Category")]
            public int CategoryId { get; set; }
        }
        #endregion
    }
}
