using Nop.Core;
using System;

namespace Nop.Plugin.Widget.LiveAnnouncement.Domain
{
    public class Announcement : BaseEntity
    {
        public string Name { get; set; }
        public string Body { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
