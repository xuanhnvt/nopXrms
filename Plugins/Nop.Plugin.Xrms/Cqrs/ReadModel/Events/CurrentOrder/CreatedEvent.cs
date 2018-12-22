using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Cqrs.ReadModel.Events.CurrentOrder
{
    public class CreatedEvent : BaseCqrsEvent
    {
        public int TableId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public CreatedEvent(Guid id, int tableId, List<OrderItem> orderItems)
        {
            Id = id;
            TableId = tableId;
            OrderItems = orderItems ?? throw new ArgumentNullException();
        }

        #region Nested Class

        public class OrderItem
        {
            public Guid Guid { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }

            public OrderItem (Guid id, int productID, int quantity)
            {
                Guid = id;
                ProductId = productID;
                Quantity = quantity;
            }
        }

        #endregion
    }
}
