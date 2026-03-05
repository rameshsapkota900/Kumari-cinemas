using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models;

namespace Kumari_cinemas.Controllers
{
    /// <summary>
    /// Users Controller - Basic Webform for User Details CRUD.
    /// Manages the USERS table: Input, Update, Delete operations.
    /// SQL Table: Users (UserID, Username, Password, FullName, Email, Address, Phone, RegistrationDate)
    /// </summary>
    public class UsersController : Controller
    {
        private readonly OracleDbHelper _db;
        public UsersController(OracleDbHelper db) { _db = db; }

        // GET: /Users — List View
        public IActionResult Index()
        {
            var users = _db.GetAllUsers();
            return View(users);
        }

        // GET: /Users/Create — Form View (Input)
        public IActionResult Create()
        {
            return View(new User { RegistrationDate = DateTime.Now });
        }

        // POST: /Users/Create — Insert into Users table
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            try
            {
                _db.InsertUser(user);
                TempData["Success"] = $"User '{user.FullName}' (ID: {user.UserID}) created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating user: " + ex.Message;
                return View(user);
            }
        }

        // GET: /Users/Edit/501 — Form View (Update)
        public IActionResult Edit(int id)
        {
            var user = _db.GetUserById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: /Users/Edit/501 — Update Users table
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            try
            {
                _db.UpdateUser(user);
                TempData["Success"] = $"User '{user.FullName}' (ID: {user.UserID}) updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating user: " + ex.Message;
                return View(user);
            }
        }

        // GET: /Users/Delete/501 — Confirmation View
        public IActionResult Delete(int id)
        {
            var user = _db.GetUserById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: /Users/DeleteConfirmed/501 — Delete from Users table
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _db.DeleteUser(id);
                TempData["Success"] = $"User (ID: {id}) deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting user: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
