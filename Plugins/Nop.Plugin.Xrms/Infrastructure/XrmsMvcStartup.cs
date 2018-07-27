using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Nop.Core.Infrastructure;
using Nop.Plugin.Xrms.Controllers;
using Nop.Plugin.Xrms.Data;
using Nop.Web.Framework.Infrastructure.Extensions;

namespace Nop.Plugin.Xrms.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring MVC on application startup
    /// </summary>
    public class XrmsMvcStartup : INopStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add and configure MVC feature
            var pluginAssembly = typeof(MaterialGroupController).GetTypeInfo().Assembly;
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(
                    new EmbeddedFileProvider(pluginAssembly));
            });
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {

        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order
        {
            //MVC should be loaded last
            get { return 1000; }
        }
    }
}
