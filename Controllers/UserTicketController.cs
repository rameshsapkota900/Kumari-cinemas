using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models.ViewModels;

namespace Kumari_cinemas.Controllers
{
    /// <summary>
    /// Complex Webform 1: User Ticket
    /// For any selected user, displays their details and tickets bought in the last 6 months.
    /// Uses ListBox / DropDownList for user selection (Template Field).
    /// OracleDbHelper.GetUserTickets() returns full UserTicketViewModel with 7-Table JOIN.
    /// </summary>
    public class UserTicketController : Controller
    {
        private readonly OracleDbHelper _db;
        public UserTicketController(OracleDbHelper db) { _db = db; }

        public IActionResult Index(int? userId)
        {
            if (userId.HasValue && userId.Value > 0)
            {
                try
                {
                    // GetUserTickets returns the full ViewModel (AllUsers, SelectedUser, Tickets)
                    var vm = _db.GetUserTickets(userId.Value);
                    return View(vm);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error loading user tickets: " + ex.Message;
                }
            }

            // No user selected — just load the user list
            var emptyVm = new UserTicketViewModel
            {
                AllUsers = _db.GetAllUsers(),
                SelectedUserId = null
            };
            return View(emptyVm);
        }
    }
}
