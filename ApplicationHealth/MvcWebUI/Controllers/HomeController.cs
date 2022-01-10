using ApplicationHealth.Domain.DataTable;
using ApplicationHealth.Domain.DataTable.Base;
using ApplicationHealth.Domain.Entities;
using ApplicationHealth.MvcWebUI.Models;
using ApplicationHealth.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ApplicationHealth.MvcWebUI.Controllers
{
    public class HomeController : CustomBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppDefService _appService;

        public HomeController(ILogger<HomeController> logger, IAppDefService appService)
        {
            _logger = logger;
            _appService = appService;
        }

        public IActionResult Index()
        {
            _logger.Log(LogLevel.Trace, "uygulama başladı");
            return View();
        }
        public IActionResult Detail(int id)
        {
            return View(_appService.GetById(id));

        }
        public IActionResult _InsertApp()
        {
            return PartialView("_InsertApp");
        }
        [HttpPost]
        public JsonResult InsertApp(AppDef app)
        {
            return new JsonResult(_appService.Add(app));
        }
        [HttpPost]
        public JsonResult DeleteApp(int id)
        {
            return new JsonResult(_appService.Delete(id));
        }
        public IActionResult _UpdateApp(int id)
        {
            return PartialView("_UpdateApp", _appService.GetById(id));
        }
        [HttpPost]
        public JsonResult UpdateApp(AppDef app)
        {
            return new JsonResult(_appService.Update(app));
        }
        [HttpPost]
        public JsonResult CheckAppIsUp(int id)
        {
            return new JsonResult(_appService.CheckAppIsUp(id));
        }

        [HttpPost]
        public AppDefDataTable GetAppDefDataTable(BaseFilterParameters filters)
        {
            return _appService.GetAppDefDataTable(LoadDataTableSortParameters(filters));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
