using Microsoft.AspNetCore.Routing;
using Nop.Core.Plugins;
using Nop.Plugin.Widget.LiveAnnouncement.Data;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widget.LiveAnnouncement
{
    public class LiveAnnouncementPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        #region Fields

        private readonly LiveAnnouncementObjectContext _context;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingContext;


        #endregion


        #region Ctr

        public LiveAnnouncementPlugin(LiveAnnouncementObjectContext context, ILocalizationService localizationService, ISettingService settingContext)
        {
            _context = context;
            _localizationService = localizationService;
            _settingContext = settingContext;
        }

        #endregion

        #region Install / Uninstall


        public override void Install()
        {
            _localizationService.AddOrUpdatePluginLocaleResource("Misc.Announcement", "Announcement Create");
            _localizationService.AddOrUpdatePluginLocaleResource("Misc.AnnouncementList", "Announcement List");


            _context.InstallSchema();
            base.Install();
        }
        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings

            //data
            _context.Uninstall();
            _localizationService.DeletePluginLocaleResource("Misc.Announcement");
            _localizationService.DeletePluginLocaleResource("Misc.AnnouncementList");



            base.Uninstall();
        }

        #endregion

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var liveAnnouncementPluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "LiveAnnouncement");
            if (liveAnnouncementPluginNode == null)
            {
                liveAnnouncementPluginNode = new SiteMapNode()
                {
                    SystemName = "Live Announcement",
                    Title = "Live Announcement",
                    Visible = true,
                    IconClass = "fa-gear"
                };
                rootNode.ChildNodes.Add(liveAnnouncementPluginNode);
            }

            liveAnnouncementPluginNode.ChildNodes.Add(new SiteMapNode()
            {
                Title = _localizationService.GetResource("Misc.Announcement"),
                Visible = true,
                IconClass = "fa-dot-circle-o",
                Url = "~/Admin/LiveAnnouncement/Announcement"
            });

            liveAnnouncementPluginNode.ChildNodes.Add(new SiteMapNode()
            {
                Title = _localizationService.GetResource("Misc.AnnouncementList"),
                Visible = true,
                IconClass = "fa-dot-circle-o",
                Url = "~/Admin/LiveAnnouncement/AnnouncementList"
            });
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string>
            {
               PublicWidgetZones.HeaderAfter
            };
        }



        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "LiveAnnouncementView";
        }
    }
}
