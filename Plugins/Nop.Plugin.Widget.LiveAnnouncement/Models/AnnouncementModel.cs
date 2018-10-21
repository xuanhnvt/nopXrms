using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widget.LiveAnnouncement.Models
{
    public partial class AnnouncementModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Name")]
        public string Name { get; set; }
        [NopResourceDisplayName("Body")]
        public string Body { get; set; }
        public bool IsActive { get; set; }
    }
}