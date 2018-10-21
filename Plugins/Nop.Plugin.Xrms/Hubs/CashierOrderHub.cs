using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Nop.Plugin.Xrms.Hubs
{
    public class CashierOrderHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            //Context.User
            //Context.GetHttpContext.
            //Clients.
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
