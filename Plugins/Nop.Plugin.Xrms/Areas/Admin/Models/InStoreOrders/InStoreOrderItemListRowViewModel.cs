using System;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders
{
    public partial class InStoreOrderItemListRowViewModel
    {
        public int Id { get; set; }
        public Guid AggregateId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int StateId { get; set; }
        public int Quantity { get; set; }
        public int OldQuantity { get; set; }

        // order info
        public int CurrentOrderId { get; set; }
        public Guid CurrentOrderGuid { get; set; }
        public int Version { get; set; }
    }
}
