using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models;

namespace Kumari_cinemas.Controllers
{
    /// <summary>
    /// Tickets Controller - Basic Webform for Ticket Details CRUD.
    /// Table: TICKETS (TicketID, Status, IssueDate, ValidTill, TicketPriceID FK, CancellationID FK, SeatID FK)
    /// Template Fields for FKs: TicketPriceID → TicketPrices, CancellationID → Cancellations, SeatID → Seats
    /// </summary>
    public class TicketsController : Controller
    {
        private readonly OracleDbHelper _db;
        public TicketsController(OracleDbHelper db) { _db = db; }

        public IActionResult Index()
        {
            var tickets = _db.GetAllTickets();
            return View(tickets);
        }

        public IActionResult Create()
        {
            ViewBag.TicketPrices = _db.GetAllTicketPrices();
            ViewBag.Seats = _db.GetAllSeats();
            ViewBag.Cancellations = _db.GetAllCancellations();
            return View(new Ticket());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Ticket ticket)
        {
            try
            {
                _db.InsertTicket(ticket);
                TempData["Success"] = $"Ticket (ID: {ticket.TicketID}) created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating ticket: " + ex.Message;
                ViewBag.TicketPrices = _db.GetAllTicketPrices();
                ViewBag.Seats = _db.GetAllSeats();
                ViewBag.Cancellations = _db.GetAllCancellations();
                return View(ticket);
            }
        }

        public IActionResult Edit(int id)
        {
            var ticket = _db.GetTicketById(id);
            if (ticket == null) return NotFound();
            ViewBag.TicketPrices = _db.GetAllTicketPrices();
            ViewBag.Seats = _db.GetAllSeats();
            ViewBag.Cancellations = _db.GetAllCancellations();
            return View(ticket);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Ticket ticket)
        {
            try
            {
                _db.UpdateTicket(ticket);
                TempData["Success"] = $"Ticket (ID: {ticket.TicketID}) updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating ticket: " + ex.Message;
                ViewBag.TicketPrices = _db.GetAllTicketPrices();
                ViewBag.Seats = _db.GetAllSeats();
                ViewBag.Cancellations = _db.GetAllCancellations();
                return View(ticket);
            }
        }

        public IActionResult Delete(int id)
        {
            var ticket = _db.GetTicketById(id);
            if (ticket == null) return NotFound();
            return View(ticket);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _db.DeleteTicket(id);
                TempData["Success"] = $"Ticket (ID: {id}) deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting ticket: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
