using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widget.LiveAnnouncement.Components
{
    [ViewComponent(Name = "LiveAnnouncementView")]
    public class AnnouncementViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            return View("~/Plugins/Widget.LiveAnnouncement/Views/LiveAnnouncementView/LiveAnnouncement.cshtml");
        }
    }
}
