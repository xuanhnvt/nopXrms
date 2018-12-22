using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Nop.Core;
using Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders;
using Nop.Plugin.Xrms.Cqrs.ReadModel.Events.CurrentOrder;
using Nop.Plugin.Xrms.Hubs;
using Nop.Services.Localization;
using Nop.Services.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// Client notification service
    /// </summary>
    public partial class ClientNotificationService : IClientNotificationService
    {
        private readonly IHubContext<CashierOrderHub> _cashierOrderHubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;

        public ClientNotificationService(IHubContext<CashierOrderHub> cashierOrderHubContext,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            ILogger logger,
            IWorkContext workContext)
        {
            _cashierOrderHubContext = cashierOrderHubContext;
            _httpContextAccessor = httpContextAccessor;
            _localizationService = localizationService;
            _logger = logger;
            _workContext = workContext;
        }

        public async Task SendMessage(string user, string message)
        {
            
            await _cashierOrderHubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task NotifyCreatedOrderEvent(NotifyCreatedOrderModel message)
        {
            await _cashierOrderHubContext.Clients.All.SendAsync("CreatedOrderEvent", message);
        }

        public async Task NotifyChangedOrderItemQuantityEvent(NotifyChangedOrderItemModel message)
        {
            var currentConnection = _httpContextAccessor.HttpContext.Session.GetString("HubConnectionId");
            await _cashierOrderHubContext.Clients.AllExcept(currentConnection).SendAsync("ChangedOrderItemQuantityEvent", message);
        }

        public async Task NotifyChangedOrderItemStateEvent(NotifyChangedOrderItemModel message)
        {
            var currentConnection = _httpContextAccessor.HttpContext.Session.GetString("HubConnectionId");
            await _cashierOrderHubContext.Clients.AllExcept(currentConnection).SendAsync("ChangedOrderItemStateEvent", message);
        }


        public async Task NotifyAddedOrderItemEvent(NotifyChangedOrderItemModel message)
        {
            var currentConnection = _httpContextAccessor.HttpContext.Session.GetString("HubConnectionId");
            //_logger.InsertLog(Core.Domain.Logging.LogLevel.Information, String.Format("Current client connection id = {0}", currentConnection));
            await _cashierOrderHubContext.Clients.AllExcept(currentConnection).SendAsync("AddedOrderItemEvent", message);
        }
    }
}
