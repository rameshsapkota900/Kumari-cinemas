using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;

namespace Kumari_cinemas.Controllers
{
    /// <summary>
    /// Home Controller - Dashboard with Attractive Graphical Display.
    /// Provides summary statistics and charts for the KumariCinemas system.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly OracleDbHelper _db;

        public HomeController(OracleDbHelper db)
        {
            _db = db;
        }

        /// <summary>
        /// Homepage with Dashboard showing system statistics and charts.
        /// </summary>
        public IActionResult Index()
        {
            try
            {
                var vm = _db.GetDashboardData();
                return View(vm);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new Models.ViewModels.DashboardViewModel());
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
