using ApplicationHealth.Domain.DataTable;
using ApplicationHealth.Domain.DataTable.Base;
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
