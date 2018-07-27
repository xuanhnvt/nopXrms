using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Customers;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.UI;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// Represents event consumer of the Worldpay payment plugin
    /// </summary>
    public class EventConsumer : IConsumer<AdminTabStripCreated>
    {
        #region Fields

        private readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public EventConsumer(ILocalizationService localizationService)
        {
            this._localizationService = localizationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle admin tabstrip created event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public async void HandleEvent(AdminTabStripCreated eventMessage)
        {
            if (eventMessage?.Helper == null)
                return;

            //we need product details page
            var tabsElementId = "product-edit";
            if (!eventMessage.TabStripName.Equals(tabsElementId))
                return;

            //check whether the plugin is installed
            //if (!(typeof(XrmsPlugin)?.PluginDescriptor?.Installed ?? false))
                //return;

            //get the view model
            if (!(eventMessage.Helper.ViewData.Model is ProductModel productModel))
                return;

            //compose script to create a new tab
            var productEditTabElementId = "tab-productEdit";
            var productEditTab = new HtmlString($@"
                <script type='text/javascript'>
                    $(document).ready(function() {{
                        $(`
                            <li>
                                <a data-tab-name='{productEditTabElementId}' data-toggle='tab' href='#{productEditTabElementId}'>
                                    {_localizationService.GetResource("Xrms.Admin.Catalog.Products.Details.Tabs.ProductRecipes")}
                                </a>
                            </li>
                        `).appendTo('#{tabsElementId} .nav-tabs:first');
                        $(`
                            <div class='tab-pane' id='{productEditTabElementId}'>
                                {
                                    (await eventMessage.Helper.PartialAsync("~/Plugins/Xrms/Areas/Admin/Views/ProductExtension/_Product._CreateOrUpdate.ProductRecipes.cshtml", productModel)).RenderHtmlContent()
                                        .Replace("</script>", "<\\/script>") //we need escape a closing script tag to prevent terminating the script block early
                                }
                            </div>
                        `).appendTo('#{tabsElementId} .tab-content:first');
                    }});
                </script>");

            //add this tab as a block to render on the customer details page
            eventMessage.BlocksToRender.Add(productEditTab);
        }

    #endregion
    }
}