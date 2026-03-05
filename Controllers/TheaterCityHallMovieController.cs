using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models.ViewModels;

namespace Kumari_cinemas.Controllers
{
    /// <summary>
    /// Complex Webform 2: TheaterCityHall Movie
    /// For any selected hall, displays theater/city info and all movies/showtimes.
    /// Uses DropDownList for hall selection (Template Field).
    /// OracleDbHelper.GetTheaterCityHallMovies() returns full TheaterCityHallMovieViewModel with 4-Table JOIN.
    /// </summary>
    public class TheaterCityHallMovieController : Controller
    {
        private readonly OracleDbHelper _db;
        public TheaterCityHallMovieController(OracleDbHelper db) { _db = db; }

        public IActionResult Index(int? hallId)
        {
            if (hallId.HasValue && hallId.Value > 0)
            {
                try
                {
                    var vm = _db.GetTheaterCityHallMovies(hallId.Value);
                    return View(vm);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error loading data: " + ex.Message;
                }
            }

            var emptyVm = new TheaterCityHallMovieViewModel
            {
                AllHalls = _db.GetAllHalls(),
                SelectedHallId = null
            };
            return View(emptyVm);
        }
    }
}
