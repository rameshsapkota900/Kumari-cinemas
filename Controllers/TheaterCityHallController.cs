using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models;
using Kumari_cinemas.Models.ViewModels;

namespace Kumari_cinemas.Controllers
{
    /// <summary>
    /// TheaterCityHall Controller - Basic Webform for TheaterCityHall Details CRUD.
    /// Manages both THEATERS and HALLS tables combined.
    /// Shows Theater+City+Hall information with Template Fields for Foreign Keys (TheaterID in Halls).
    /// </summary>
    public class TheaterCityHallController : Controller
    {
        private readonly OracleDbHelper _db;
        public TheaterCityHallController(OracleDbHelper db) { _db = db; }

        // GET: /TheaterCityHall — Combined List View of Theaters and Halls
        public IActionResult Index()
        {
            var vm = new TheaterCityHallViewModel
            {
                Theaters = _db.GetAllTheaters(),
                Halls = _db.GetAllHalls()
            };
            return View(vm);
        }

        // ---- THEATER CRUD ----

        public IActionResult CreateTheater() => View(new Theater());

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateTheater(Theater theater)
        {
            try
            {
                _db.InsertTheater(theater);
                TempData["Success"] = $"Theater '{theater.Name}' (ID: {theater.TheaterID}) created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating theater: " + ex.Message;
                return View(theater);
            }
        }

        public IActionResult EditTheater(int id)
        {
            var theater = _db.GetTheaterById(id);
            if (theater == null) return NotFound();
            return View(theater);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditTheater(Theater theater)
        {
            try
            {
                _db.UpdateTheater(theater);
                TempData["Success"] = $"Theater '{theater.Name}' (ID: {theater.TheaterID}) updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating theater: " + ex.Message;
                return View(theater);
            }
        }

        public IActionResult DeleteTheater(int id)
        {
            var theater = _db.GetTheaterById(id);
            if (theater == null) return NotFound();
            return View(theater);
        }

        [HttpPost, ActionName("DeleteTheater"), ValidateAntiForgeryToken]
        public IActionResult DeleteTheaterConfirmed(int id)
        {
            try
            {
                _db.DeleteTheater(id);
                TempData["Success"] = $"Theater (ID: {id}) deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting theater: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // ---- HALL CRUD ----

        public IActionResult CreateHall()
        {
            ViewBag.Theaters = _db.GetAllTheaters();
            return View(new Hall());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateHall(Hall hall)
        {
            try
            {
                _db.InsertHall(hall);
                TempData["Success"] = $"Hall '{hall.HallName}' (ID: {hall.HallID}) created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating hall: " + ex.Message;
                ViewBag.Theaters = _db.GetAllTheaters();
                return View(hall);
            }
        }

        public IActionResult EditHall(int id)
        {
            var hall = _db.GetHallById(id);
            if (hall == null) return NotFound();
            ViewBag.Theaters = _db.GetAllTheaters();
            return View(hall);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditHall(Hall hall)
        {
            try
            {
                _db.UpdateHall(hall);
                TempData["Success"] = $"Hall '{hall.HallName}' (ID: {hall.HallID}) updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating hall: " + ex.Message;
                ViewBag.Theaters = _db.GetAllTheaters();
                return View(hall);
            }
        }

        public IActionResult DeleteHall(int id)
        {
            var hall = _db.GetHallById(id);
            if (hall == null) return NotFound();
            return View(hall);
        }

        [HttpPost, ActionName("DeleteHall"), ValidateAntiForgeryToken]
        public IActionResult DeleteHallConfirmed(int id)
        {
            try
            {
                _db.DeleteHall(id);
                TempData["Success"] = $"Hall (ID: {id}) deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting hall: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
