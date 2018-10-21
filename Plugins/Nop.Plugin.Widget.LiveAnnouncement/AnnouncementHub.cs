using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Nop.Plugin.LiveAnnouncement
{
    public class AnnouncementHub : Hub
    {
        public Task Send(string announcement)
        {
            return Clients.All.SendAsync("Send", announcement);
        }
    }
}

