using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Nop.Plugin.LiveAnnouncement;
using Nop.Plugin.Widget.LiveAnnouncement.Domain;
using Nop.Plugin.Widget.LiveAnnouncement.Models;
using Nop.Plugin.Widget.LiveAnnouncement.Services;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using System;
using System.Linq;

namespace Nop.Plugin.Widget.LiveAnnouncement.Controllers
{

    public class LiveAnnouncementController : BaseAdminController
    {
        #region Field


        private readonly IAnnouncementService _announcementService;
        private IHubContext<AnnouncementHub> _announcementHubContext;

        #endregion

        #region Ctr

        public LiveAnnouncementController(
            IAnnouncementService announcementService,
            IHubContext<AnnouncementHub> announcementHubContext)
        {
            _announcementService = announcementService;
            _announcementHubContext = announcementHubContext;
        }

        #endregion

        #region Methods        

        public IActionResult Announcement()
        {
            var model = new AnnouncementModel();
            return View("~/Plugins/Widget.LiveAnnouncement/Views/LiveAnnouncementView/Announcement.cshtml", model);
        }

        [HttpPost]
        public IActionResult Announcement(AnnouncementModel model)
        {
            Announcement objOfAnnouncementDomain = new Announcement();
            objOfAnnouncementDomain.Name = model.Name;
            objOfAnnouncementDomain.Body = model.Body;
            objOfAnnouncementDomain.IsActive = model.IsActive;
            objOfAnnouncementDomain.CreateDate = DateTime.UtcNow;

            _announcementService.Insert(objOfAnnouncementDomain);

            if (model.IsActive == true)
            {
                _announcementHubContext.Clients.All.SendAsync("send", model.Body.ToString());

            }

            return RedirectToAction("AnnouncementList");
        }
        [HttpPost]
        public IActionResult Edit(AnnouncementModel model)
        {
            var entity = _announcementService.GetAnnouncementById(model.Id);


            entity.Name = model.Name;
            entity.Body = model.Body;
            entity.IsActive = model.IsActive;
            entity.CreateDate = DateTime.UtcNow;

            _announcementService.Update(entity);

            if (model.IsActive == true)
            {
                _announcementHubContext.Clients.All.SendAsync("send", model.Body.ToString());
            }
            return RedirectToAction("AnnouncementList");
        }

        public IActionResult Edit(int Id)
        {
            var singleAnnouncement = _announcementService.GetAnnouncementById(Id);

            var model = new AnnouncementModel();
            model.Id = singleAnnouncement.Id;
            model.Name = singleAnnouncement.Name;
            model.Body = singleAnnouncement.Body;
            model.IsActive = singleAnnouncement.IsActive;

            return View("~/Plugins/Widget.LiveAnnouncement/Views/LiveAnnouncementView/Announcement.cshtml", model);

        }


        public IActionResult Delete(int Id)
        {
            var singleAnnouncement = _announcementService.GetAnnouncementById(Id);
            _announcementService.Delete(singleAnnouncement);

            return new NullJsonResult();
        }


        public IActionResult AnnouncementList()
        {
            var model = new AnnouncementModel();
            return View("~/Plugins/Widget.LiveAnnouncement/Views/LiveAnnouncementView/AnnouncementList.cshtml", model);
        }


        [HttpPost]
        public IActionResult AnnouncementList(DataSourceRequest command, AnnouncementModel model)
        {

            var announcementPagedList = _announcementService.GetAnnouncementDomain(pageIndex: command.Page - 1, pageSize: command.PageSize);
            var gridModel = new DataSourceResult();
            gridModel.Data = announcementPagedList.Select(x =>
            {
                return new AnnouncementModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Body = x.Body,
                    IsActive = x.IsActive
                };
            });
            gridModel.Total = announcementPagedList.TotalCount;
            return Json(gridModel);
        }
        #endregion
    }
}
