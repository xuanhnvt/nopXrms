using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders
{
    public partial class InStoreOrderViewModel
    {
        public InStoreOrderViewModel()
        {
            AvailableTables = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
        }

        public Guid AggregateId { get; set; }

        public int Version { get; set; }

        public int Id { get; set; }

        public int OrderId { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.Table")]
        public int TableId { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.Table")]
        public string TableName { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.Code")]
        public string OrderCode { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.State")]
        public int StateId { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.PrintCount")]
        public int PrintCount { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.BilledOnUtc")]
        public DateTime BilledOnUtc { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.CheckedOutOnUtc")]
        public DateTime CheckedOutOnUtc { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.CreatedOnUtc")]
        public DateTime CreatedOnUtc { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.UpdatedOnUtc")]
        public DateTime UpdatedOnUtc { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.SubTotalPrice")]
        public decimal SubTotalPrice { get; set; }

        public IList<SelectListItem> AvailableTables { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }
    }
}
