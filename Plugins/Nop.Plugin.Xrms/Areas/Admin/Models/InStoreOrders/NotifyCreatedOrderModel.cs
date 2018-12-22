using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders
{
    public partial class NotifyCreatedOrderModel : NotifyChangedOrderBaseModel
    {
        public List<InStoreOrderItemListRowViewModel> AddedOrderItems { get; set; }

        public NotifyCreatedOrderModel ()
        {
            AddedOrderItems = new List<InStoreOrderItemListRowViewModel>();
        }
    }
}
