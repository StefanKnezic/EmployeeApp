using EmployeeApp.DataAccess;
using EmployeeApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _dbContext;
        public HomeController(ILogger<HomeController> logger,DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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