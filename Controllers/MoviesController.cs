using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models;

namespace Kumari_cinemas.Controllers
{
    /// <summary>
    /// Movies Controller - Basic Webform for Movie Details CRUD.
    /// Table: MOVIES (MovieID, Title, Duration, Language, Genre, ReleaseDate)
    /// </summary>
    public class MoviesController : Controller
    {
        private readonly OracleDbHelper _db;
        public MoviesController(OracleDbHelper db) { _db = db; }

        public IActionResult Index()
        {
            var movies = _db.GetAllMovies();
            return View(movies);
        }

        public IActionResult Create() => View(new Movie());

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie)
        {
            try
            {
                _db.InsertMovie(movie);
                TempData["Success"] = $"Movie '{movie.Title}' (ID: {movie.MovieID}) created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating movie: " + ex.Message;
                return View(movie);
            }
        }

        public IActionResult Edit(int id)
        {
            var movie = _db.GetMovieById(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Movie movie)
        {
            try
            {
                _db.UpdateMovie(movie);
                TempData["Success"] = $"Movie '{movie.Title}' (ID: {movie.MovieID}) updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating movie: " + ex.Message;
                return View(movie);
            }
        }

        public IActionResult Delete(int id)
        {
            var movie = _db.GetMovieById(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _db.DeleteMovie(id);
                TempData["Success"] = $"Movie (ID: {id}) deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting movie: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
