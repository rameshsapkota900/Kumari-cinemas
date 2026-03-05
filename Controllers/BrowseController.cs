using Microsoft.AspNetCore.Mvc;
using Kumari_cinemas.Data;
using Kumari_cinemas.Models.ViewModels;

namespace Kumari_cinemas.Controllers
{
    /// <summary>
    /// Browse Controller - Customer-Facing Booking Interface
    /// Handles the complete booking workflow:
    /// 1. Browse Theaters
    /// 2. Select Hall & View Movie Schedule
    /// 3. Select Show & View Seats
    /// 4. Select Seats & Review Booking
    /// 5. Complete Payment
    /// </summary>
    public class BrowseController : Controller
    {
        private readonly OracleDbHelper _db;

        public BrowseController(OracleDbHelper db)
        {
            _db = db;
        }

        /// <summary>
        /// Step 1: Browse All Theaters
        /// Shows all available theaters with their cities and locations
        /// </summary>
        public IActionResult Theaters()
        {
            try
            {
                var theaters = _db.GetAllTheaters();
                ViewBag.Title = "Select Theater";
                ViewBag.Description = "Choose your preferred cinema location";
                return View(theaters);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading theaters: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Step 2: Browse Halls for Selected Theater
        /// Shows all halls in the selected theater with capacities
        /// </summary>
        public IActionResult Halls(int theaterId)
        {
            try
            {
                var theater = _db.GetTheaterById(theaterId);
                if (theater == null)
                {
                    TempData["Error"] = "Theater not found";
                    return RedirectToAction(nameof(Theaters));
                }

                var halls = _db.GetTheaterHallsForBrowse(theaterId);
                ViewBag.TheaterName = theater.Name;
                ViewBag.TheaterCity = theater.City;
                ViewBag.TheaterID = theaterId;
                ViewBag.Title = $"Halls in {theater.Name}";
                return View(halls);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading halls: " + ex.Message;
                return RedirectToAction(nameof(Theaters));
            }
        }

        /// <summary>
        /// Step 3: Browse Movie Shows for Selected Hall
        /// Shows all upcoming movies and showtimes for the selected hall
        /// </summary>
        public IActionResult Shows(int hallId)
        {
            try
            {
                var vm = _db.GetTheaterCityHallMovies(hallId);
                if (vm.TheaterCityHallInfo == null)
                {
                    TempData["Error"] = "Hall not found";
                    return RedirectToAction(nameof(Theaters));
                }

                ViewBag.Title = $"Shows in {vm.TheaterCityHallInfo.HallName}";
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading shows: " + ex.Message;
                return RedirectToAction(nameof(Theaters));
            }
        }

        /// <summary>
        /// Step 4: Select Seats for a Show
        /// Displays interactive seat map for the user to select seats
        /// </summary>
        public IActionResult SelectSeats(int showId)
        {
            try
            {
                var show = _db.GetShowById(showId);
                if (show == null)
                {
                    TempData["Error"] = "Show not found";
                    return RedirectToAction(nameof(Theaters));
                }

                var movie = _db.GetMovieById(show.MovieID);
                var hall = _db.GetHallById(show.HallID);
                var theater = _db.GetTheaterById(hall.TheaterID);
                var seats = _db.GetHallSeatsWithStatus(showId);
                var prices = _db.GetTicketPrices();

                var vm = new SeatSelectionViewModel
                {
                    ShowID = showId,
                    MovieTitle = movie?.Title ?? "Unknown Movie",
                    TheaterName = theater?.Name ?? "Unknown Theater",
                    HallName = hall?.HallName ?? "Unknown Hall",
                    ShowDate = show.ShowDate,
                    ShowTime = show.ShowTime,
                    Seats = seats,
                    TicketPrices = prices,
                    SelectedSeats = new List<int>(),
                    MaxSeats = 10
                };

                ViewBag.Title = $"Select Seats - {movie?.Title}";
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading seats: " + ex.Message;
                return RedirectToAction(nameof(Theaters));
            }
        }

        /// <summary>
        /// Step 5: Review Booking & Process Payment
        /// Shows booking summary and payment options
        /// </summary>
        [HttpPost]
        public IActionResult ReviewBooking(int showId, [FromBody] SeatBookingRequest request)
        {
            try
            {
                if (request?.SelectedSeatIds == null || request.SelectedSeatIds.Count == 0)
                {
                    return Json(new { success = false, message = "Please select at least one seat" });
                }

                if (request.SelectedSeatIds.Count > 10)
                {
                    return Json(new { success = false, message = "Maximum 10 seats can be booked at once" });
                }

                var show = _db.GetShowById(showId);
                var movie = _db.GetMovieById(show.MovieID);
                var hall = _db.GetHallById(show.HallID);
                var theater = _db.GetTheaterById(hall.TheaterID);
                var seats = _db.GetSeatsByIds(request.SelectedSeatIds);
                var prices = _db.GetTicketPrices();

                decimal totalPrice = 0;
                foreach (var seat in seats)
                {
                    var price = prices.FirstOrDefault(p => p.TicketPriceID == seat.TicketPriceID);
                    if (price != null)
                    {
                        totalPrice += price.Price;
                    }
                }

                var vm = new BookingReviewViewModel
                {
                    ShowID = showId,
                    MovieTitle = movie?.Title ?? "Unknown",
                    TheaterName = theater?.Name ?? "Unknown",
                    HallName = hall?.HallName ?? "Unknown",
                    ShowDate = show.ShowDate,
                    ShowTime = show.ShowTime,
                    SelectedSeats = seats.Select(s => s.SeatNumber).ToList(),
                    TotalAmount = totalPrice,
                    PaymentMethod = "Credit Card",
                    BookingDate = DateTime.Now
                };

                TempData["BookingData"] = System.Text.Json.JsonSerializer.Serialize(vm);
                return Json(new { success = true, message = "Proceeding to payment" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error processing booking: " + ex.Message });
            }
        }

        /// <summary>
        /// Step 6: Complete Payment
        /// Processes payment and creates booking
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompletePayment(PaymentRequest paymentRequest)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    TempData["Error"] = "Please login to complete booking";
                    return RedirectToAction("Login", "Account");
                }

                // Validate payment details
                if (string.IsNullOrEmpty(paymentRequest.CardNumber) || 
                    string.IsNullOrEmpty(paymentRequest.CardHolder) ||
                    string.IsNullOrEmpty(paymentRequest.ExpiryDate) ||
                    string.IsNullOrEmpty(paymentRequest.CVV))
                {
                    ModelState.AddModelError("", "All payment fields are required");
                    return View(paymentRequest);
                }

                // Create booking in database
                int bookingId = _db.CreateBooking(
                    userId: int.Parse(User.FindFirst("UserId")?.Value ?? "0"),
                    totalAmount: paymentRequest.TotalAmount,
                    paymentStatus: "Pending"
                );

                // Create tickets for selected seats
                var vm = System.Text.Json.JsonSerializer.Deserialize<BookingReviewViewModel>(
                    (string)TempData["BookingData"]);

                foreach (var seat in vm.SelectedSeats)
                {
                    _db.CreateTicket(bookingId, vm.ShowID, seat);
                }

                // Process payment
                int paymentId = _db.CreatePayment(
                    bookingId: bookingId,
                    amount: paymentRequest.TotalAmount,
                    status: "Pending",
                    paymentMethod: paymentRequest.PaymentMethod
                );

                TempData["Success"] = "Booking confirmed! Payment is pending admin approval.";
                return RedirectToAction("BookingConfirmation", new { bookingId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error processing payment: " + ex.Message;
                return View(paymentRequest);
            }
        }

        /// <summary>
        /// Booking Confirmation Page
        /// Shows booking details and ticket information
        /// </summary>
        public IActionResult BookingConfirmation(int bookingId)
        {
            try
            {
                var booking = _db.GetBookingById(bookingId);
                if (booking == null)
                {
                    TempData["Error"] = "Booking not found";
                    return RedirectToAction(nameof(Theaters));
                }

                var tickets = _db.GetBookingTickets(bookingId);
                ViewBag.Title = "Booking Confirmed";
                return View(new { Booking = booking, Tickets = tickets });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading booking: " + ex.Message;
                return RedirectToAction(nameof(Theaters));
            }
        }

        /// <summary>
        /// View My Bookings
        /// Shows all bookings and tickets for the logged-in user
        /// </summary>
        public IActionResult MyBookings()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var bookings = _db.GetUserBookings(userId);
                ViewBag.Title = "My Bookings";
                return View(bookings);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading bookings: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }

    // ============================================================================
    // VIEW MODELS FOR BOOKING WORKFLOW
    // ============================================================================

    public class SeatSelectionViewModel
    {
        public int ShowID { get; set; }
        public string MovieTitle { get; set; }
        public string TheaterName { get; set; }
        public string HallName { get; set; }
        public DateTime ShowDate { get; set; }
        public string ShowTime { get; set; }
        public List<SeatDetails> Seats { get; set; }
        public List<TicketPriceModel> TicketPrices { get; set; }
        public List<int> SelectedSeats { get; set; }
        public int MaxSeats { get; set; }
    }

    public class SeatBookingRequest
    {
        public List<int> SelectedSeatIds { get; set; }
    }

    public class BookingReviewViewModel
    {
        public int ShowID { get; set; }
        public string MovieTitle { get; set; }
        public string TheaterName { get; set; }
        public string HallName { get; set; }
        public DateTime ShowDate { get; set; }
        public string ShowTime { get; set; }
        public List<string> SelectedSeats { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime BookingDate { get; set; }
    }

    public class PaymentRequest
    {
        public int ShowID { get; set; }
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class SeatDetails
    {
        public int SeatID { get; set; }
        public string SeatNumber { get; set; }
        public string Status { get; set; }
        public int TicketPriceID { get; set; }
        public string Category { get; set; }
    }

    public class TicketPriceModel
    {
        public int TicketPriceID { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}
