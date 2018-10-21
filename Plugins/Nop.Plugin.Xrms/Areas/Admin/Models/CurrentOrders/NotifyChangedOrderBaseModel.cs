using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders
{
    public abstract class NotifyChangedOrderBaseModel
    {
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; }
        public string OrderCode { get; set; }
        public int State { get; set; }
        public int PrintCount { get; set; }
        public string BilledOnUtc { get; set; }
        public string CheckedOutOnUtc { get; set; }
        public string CreatedOnUtc { get; set; }
        public string UpdatedOnUtc { get; set; }
        public decimal SubTotalPrice { get; set; }
    }
}
