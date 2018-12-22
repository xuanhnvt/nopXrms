using CQRSlite.Events;
using Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders;
using Nop.Plugin.Xrms.Cqrs.ReadModel.Events.CurrentOrder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// Notify to client about change
    /// </summary>
    public partial interface IClientNotificationService
    {
        Task SendMessage(string user, string message);
        Task NotifyCreatedOrderEvent(NotifyCreatedOrderModel message);
        Task NotifyAddedOrderItemEvent(NotifyChangedOrderItemModel message);
        Task NotifyChangedOrderItemQuantityEvent(NotifyChangedOrderItemModel message);
        Task NotifyChangedOrderItemStateEvent(NotifyChangedOrderItemModel message);
    }
}
