using ApplicationHealth.Domain.DataTable;
using ApplicationHealth.Domain.DataTable.Base;
using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.ViewModels;
using ApplicationHealth.MvcWebUI.Models;
using ApplicationHealth.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ApplicationHealth.MvcWebUI.Controllers
{
    public class NotificationController : CustomBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppContactService _appContactService;
        private readonly IAppNotificationService _notificationService;

        public NotificationController(ILogger<HomeController> logger, IAppContactService appContactService, IAppNotificationService notificationService)
        {
            _logger = logger;
            _appContactService = appContactService;
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            _logger.Log(LogLevel.Trace, "Notification Access");
            return View();
        }
        public IActionResult _AddNotificationContactToApp(int id)
        {
            var dto = new AppContactDTO(id, _appContactService.GetAll(m=> m.AppDefId == id));
            return PartialView("_AddNotificationContactToApp", dto);
        }
        [HttpPost]
        public JsonResult AddNotificationContactToApp(AppContact contact)
        {
            return new JsonResult(_appContactService.Add(contact));
        }

        [HttpPost]
        public AppNotificationDataTable GetAppDefDataTable(BaseFilterParameters filters)
        {
            return _notificationService.GetNotificationDataTable(LoadDataTableSortParameters(filters));
        }


    }
}
