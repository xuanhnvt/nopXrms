using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders
{
    public partial class NotifyChangedOrderItemModel : NotifyChangedOrderBaseModel
    {
        public CurrentOrderDetailsPageViewModel.OrderItemViewModel ChangedOrderItem;
    }
}
