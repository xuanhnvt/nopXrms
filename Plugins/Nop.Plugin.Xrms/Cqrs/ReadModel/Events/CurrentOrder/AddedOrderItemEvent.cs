using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Cqrs.ReadModel.Events.CurrentOrder
{
    public class AddedOrderItemEvent : BaseCqrsEvent
    {
        public Guid OrderItemGuid { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public AddedOrderItemEvent(Guid id, Guid orderItemId, int productId, int quantity)
        {
            Id = id;
            OrderItemGuid = orderItemId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
