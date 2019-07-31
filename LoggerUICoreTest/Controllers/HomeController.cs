using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoggerUICoreTest.Models;
using Microsoft.Extensions.Logging;

namespace LoggerUICoreTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger1;

        public HomeController(ILogger<HomeController> logger1)
        {
           
            _logger1 = logger1;
            _logger1.LogError("TESTE AFONSO 1");

        }
        public IActionResult Index()
        {
            _logger1.LogError("TESTE AFONSO Index");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger1.LogError("TESTE AFONSO Privacy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
