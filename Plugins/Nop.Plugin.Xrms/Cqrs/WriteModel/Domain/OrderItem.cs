using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Domain
{
    public class OrderItem
    {
        public Guid OrderItemGuid { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int State { get; set; }
    }
}
