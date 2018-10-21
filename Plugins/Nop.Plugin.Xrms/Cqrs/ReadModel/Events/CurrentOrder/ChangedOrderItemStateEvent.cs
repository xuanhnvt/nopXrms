using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Cqrs.ReadModel.Events.CurrentOrder
{
    public class ChangedOrderItemStateEvent : BaseCqrsEvent
    {
        public Guid OrderItemGuid { get; set; }
        public int ProductId { get; set; }
        public int State { get; set; }

        public ChangedOrderItemStateEvent(Guid id, Guid orderItemId, int productId, int state)
        {
            Id = id;
            OrderItemGuid = orderItemId;
            ProductId = productId;
            State = state;
        }
    }
}
