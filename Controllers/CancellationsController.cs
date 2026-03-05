using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models;

namespace Kumari_cinemas.Controllers
{
    public class CancellationsController : Controller
    {
        private readonly OracleDbHelper _db;
        public CancellationsController(OracleDbHelper db) { _db = db; }

        public IActionResult Index()
        {
            var cancellations = _db.GetAllCancellations();
            return View(cancellations);
        }

        public IActionResult Create() => View(new Cancellation { CancelDate = DateTime.Now });

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Cancellation cancellation)
        {
            try
            {
                _db.InsertCancellation(cancellation);
                TempData["Success"] = "Cancellation record created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating cancellation: " + ex.Message;
                return View(cancellation);
            }
        }

        public IActionResult Edit(int id)
        {
            var c = _db.GetCancellationById(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Cancellation cancellation)
        {
            try
            {
                _db.UpdateCancellation(cancellation);
                TempData["Success"] = "Cancellation record updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating cancellation: " + ex.Message;
                return View(cancellation);
            }
        }

        public IActionResult Delete(int id)
        {
            var c = _db.GetCancellationById(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _db.DeleteCancellation(id);
                TempData["Success"] = "Cancellation record deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting cancellation: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
