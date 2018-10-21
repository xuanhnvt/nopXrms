using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widget.LiveAnnouncement.Domain;
using System.Collections.Generic;
using Nop.Services.Events;

namespace Nop.Plugin.Widget.LiveAnnouncement.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        #region Field
        private readonly IRepository<Announcement> _announcementRepository;
        #endregion
        #region Ctr
        public AnnouncementService(IRepository<Announcement> announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }
        #endregion
        #region Methods
        public void Delete(Announcement AnnouncementDomain)
        {
            _announcementRepository.Delete(AnnouncementDomain);
        }


        public bool Update(Announcement AnnouncementDomain)
        {
            if (AnnouncementDomain == null)
                throw new ArgumentNullException("customer");

            _announcementRepository.Update(AnnouncementDomain);
            return true;
        }


        public void Insert(Announcement item)
        {
            _announcementRepository.Insert(item);
        }

        public IPagedList<Announcement> GetAnnouncementDomain(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from c in _announcementRepository.Table
                        select c;
            query = query.OrderBy(b => b.IsActive);
            var liveAnnouncementDomain = new PagedList<Announcement>(query, pageIndex, pageSize);
            return liveAnnouncementDomain;
        }

        public Announcement GetAnnouncementDesignFirst()
        {
            var query = from c in _announcementRepository.Table
                        where c.IsActive == true
                        orderby c.CreateDate descending
                        select c;
            var LatestAnnouncement = query.ToList().FirstOrDefault();
            return LatestAnnouncement;
        }



        public Announcement GetAnnouncementById(int Id)
        {
            return _announcementRepository.GetById(Id);
        }
        #endregion    
    }
}
