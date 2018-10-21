using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.LiveAnnouncement;
using System;

namespace Nop.Plugin.Widget.LiveAnnouncement
{
    public class AnnouncementHubAtStartUp : INopStartup
    {
        public int Order => 999;

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR(hubOptions =>
            {
                hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
            });
        }

        public void Configure(IApplicationBuilder application)
        {
            application.UseSignalR(routes =>
            {
                routes.MapHub<AnnouncementHub>("/announcement");
            });
        }


    }
}
