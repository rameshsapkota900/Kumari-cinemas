using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models;

namespace Kumari_cinemas.Controllers
{
    public class SeatsController : Controller
    {
        private readonly OracleDbHelper _db;
        public SeatsController(OracleDbHelper db) { _db = db; }

        public IActionResult Index()
        {
            var seats = _db.GetAllSeats();
            // Enrich with hall names
            var halls = _db.GetAllHalls();
            ViewBag.HallLookup = halls.ToDictionary(h => h.HallID, h => $"{h.HallName} ({h.TheaterName})");
            return View(seats);
        }

        public IActionResult Create()
        {
            ViewBag.Halls = _db.GetAllHalls();
            return View(new Seat { Status = "Available" });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Seat seat)
        {
            try
            {
                _db.InsertSeat(seat);
                TempData["Success"] = $"Seat '{seat.SeatNumber}' created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating seat: " + ex.Message;
                ViewBag.Halls = _db.GetAllHalls();
                return View(seat);
            }
        }

        public IActionResult Edit(int id)
        {
            var seat = _db.GetSeatById(id);
            if (seat == null) return NotFound();
            ViewBag.Halls = _db.GetAllHalls();
            return View(seat);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Seat seat)
        {
            try
            {
                _db.UpdateSeat(seat);
                TempData["Success"] = $"Seat '{seat.SeatNumber}' updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating seat: " + ex.Message;
                ViewBag.Halls = _db.GetAllHalls();
                return View(seat);
            }
        }

        public IActionResult Delete(int id)
        {
            var seat = _db.GetSeatById(id);
            if (seat == null) return NotFound();
            // Get hall name for display
            var halls = _db.GetAllHalls();
            ViewBag.HallLookup = halls.ToDictionary(h => h.HallID, h => $"{h.HallName}");
            return View(seat);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _db.DeleteSeat(id);
                TempData["Success"] = "Seat deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting seat: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
