using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models;

namespace Kumari_cinemas.Controllers
{
    public class TicketPricesController : Controller
    {
        private readonly OracleDbHelper _db;
        public TicketPricesController(OracleDbHelper db) { _db = db; }

        public IActionResult Index()
        {
            var prices = _db.GetAllTicketPrices();
            return View(prices);
        }

        public IActionResult Create() => View(new TicketPrice());

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(TicketPrice tp)
        {
            try
            {
                _db.InsertTicketPrice(tp);
                TempData["Success"] = $"Ticket price '{tp.Category}' created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating ticket price: " + ex.Message;
                return View(tp);
            }
        }

        public IActionResult Edit(int id)
        {
            var tp = _db.GetTicketPriceById(id);
            if (tp == null) return NotFound();
            return View(tp);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(TicketPrice tp)
        {
            try
            {
                _db.UpdateTicketPrice(tp);
                TempData["Success"] = $"Ticket price '{tp.Category}' updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating ticket price: " + ex.Message;
                return View(tp);
            }
        }

        public IActionResult Delete(int id)
        {
            var tp = _db.GetTicketPriceById(id);
            if (tp == null) return NotFound();
            return View(tp);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _db.DeleteTicketPrice(id);
                TempData["Success"] = "Ticket price deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting ticket price: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
