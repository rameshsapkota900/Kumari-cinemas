using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models.ViewModels;

namespace Kumari_cinemas.Controllers
{
    /// <summary>
    /// Complex Webform 3: Movie TheaterCityHall Occupancy Performer
    /// For any selected movie, shows top 3 theater/halls by seat occupancy %.
    /// Only paid tickets count as seat occupancy.
    /// OracleDbHelper.GetOccupancyPerformer() returns full OccupancyViewModel with nested query ROWNUM &lt;= 3.
    /// </summary>
    public class OccupancyController : Controller
    {
        private readonly OracleDbHelper _db;
        public OccupancyController(OracleDbHelper db) { _db = db; }

        public IActionResult Index(int? movieId)
        {
            if (movieId.HasValue && movieId.Value > 0)
            {
                try
                {
                    var vm = _db.GetOccupancyPerformer(movieId.Value);
                    return View(vm);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error loading occupancy data: " + ex.Message;
                }
            }

            var emptyVm = new OccupancyViewModel
            {
                AllMovies = _db.GetAllMovies(),
                SelectedMovieId = null
            };
            return View(emptyVm);
        }
    }
}
