using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models;

namespace Kumari_cinemas.Controllers
{
    /// <summary>
    /// Showtimes Controller - Basic Webform for Showtimes Details CRUD.
    /// Table: SHOWS (ShowID, ShowDate, ShowTime, ShowStatus, MovieID FK, HallID FK)
    /// Template Fields for FKs: MovieID → Movies, HallID → Halls
    /// </summary>
    public class ShowtimesController : Controller
    {
        private readonly OracleDbHelper _db;
        public ShowtimesController(OracleDbHelper db) { _db = db; }

        public IActionResult Index()
        {
            var shows = _db.GetAllShows();
            return View(shows);
        }

        public IActionResult Create()
        {
            ViewBag.Movies = _db.GetAllMovies();
            ViewBag.Halls = _db.GetAllHalls();
            return View(new Show());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Show show)
        {
            try
            {
                _db.InsertShow(show);
                TempData["Success"] = $"Show (ID: {show.ShowID}) created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating show: " + ex.Message;
                ViewBag.Movies = _db.GetAllMovies();
                ViewBag.Halls = _db.GetAllHalls();
                return View(show);
            }
        }

        public IActionResult Edit(int id)
        {
            var show = _db.GetShowById(id);
            if (show == null) return NotFound();
            ViewBag.Movies = _db.GetAllMovies();
            ViewBag.Halls = _db.GetAllHalls();
            return View(show);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Show show)
        {
            try
            {
                _db.UpdateShow(show);
                TempData["Success"] = $"Show (ID: {show.ShowID}) updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating show: " + ex.Message;
                ViewBag.Movies = _db.GetAllMovies();
                ViewBag.Halls = _db.GetAllHalls();
                return View(show);
            }
        }

        public IActionResult Delete(int id)
        {
            var show = _db.GetShowById(id);
            if (show == null) return NotFound();
            return View(show);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _db.DeleteShow(id);
                TempData["Success"] = $"Show (ID: {id}) deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting show: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
