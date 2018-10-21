using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders
{
    public partial class CurrentOrderDetailsPageViewModel
    {
        public CurrentOrderDetailsPageViewModel()
        {
            SearchModel = new SearchProductsModel();
            AvailableTables = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
        }

        public Guid AggregateId { get; set; }

        public int Version { get; set; }

        public int Id { get; set; }

        public int OrderId { get; set; }
        
        [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Fields.Table")]
        public int TableId { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Fields.Table")]
        public string TableName { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Fields.Code")]
        public string OrderCode { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Fields.State")]
        public int State { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Fields.PrintCount")]
        public int PrintCount { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Fields.BilledOnUtc")]
        public DateTime BilledOnUtc { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Fields.CheckedOutOnUtc")]
        public DateTime CheckedOutOnUtc { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Fields.CreatedOnUtc")]
        public DateTime CreatedOnUtc { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Fields.UpdatedOnUtc")]
        public DateTime UpdatedOnUtc { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Fields.SubTotalPrice")]
        public decimal SubTotalPrice { get; set; }

        public SearchProductsModel SearchModel { get; set; }

        public IList<SelectListItem> AvailableTables { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        #region Nested classes

        public partial class OrderItemViewModel
        {
            public Guid AggregateId { get; set; }
            public int Id { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal ProductPrice { get; set; }
            public int Quantity { get; set; }
            public int State { get; set; }

            public int OldQuantity { get; set; }

            // order info
            public int CurrentOrderId { get; set; }
            public Guid CurrentOrderGuid { get; set; }
            public int Version { get; set; }
        }

        public partial class ProductListItemViewModel : BaseNopModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
        
        public partial class SearchProductsModel : BaseNopModel
        {
            [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Details.SearchProducts.ProductName")]
            public string SearchProductName { get; set; }
            [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.Details.SearchProducts.Category")]
            public int SearchCategoryId { get; set; }
        }

        #endregion
    }
}
