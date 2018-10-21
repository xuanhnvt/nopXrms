using System;
using Nop.Core;
using Nop.Plugin.Widget.LiveAnnouncement.Domain;
using System.Collections.Generic;

namespace Nop.Plugin.Widget.LiveAnnouncement.Services
{
    public interface IAnnouncementService
    {
        void Delete(Announcement AnnouncementDomain);
        void Insert(Announcement item);
        bool Update(Announcement AnnouncementDomain);
        IPagedList<Announcement> GetAnnouncementDomain(int pageIndex = 0, int pageSize = int.MaxValue);
        Announcement GetAnnouncementDesignFirst();
        Announcement GetAnnouncementById(int Id);
    }
}
