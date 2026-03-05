namespace Kumari_cinemas.Models.ViewModels
{
    /// <summary>
    /// ViewModel for the Complex Form: MovieTheaterCityHallOccupancyPerformer.
    /// For any movie, show the details of top 3 theatercityhall who has maximum
    /// seat occupancy based on percentage. NOTE: Only paid tickets count as seat occupancy.
    /// SQL Query:
    ///   SELECT * FROM (
    ///       SELECT t.TheaterID, t.Name AS TheaterName, t.City, t.Address,
    ///              h.HallID, h.HallName, h.Capacity,
    ///              COUNT(tk.TicketID) AS PaidTickets,
    ///              ROUND(COUNT(tk.TicketID) * 100.0 / h.Capacity, 2) AS OccupancyPercentage
    ///       FROM Movies m
    ///       JOIN Shows sh ON m.MovieID = sh.MovieID
    ///       JOIN Halls h ON sh.HallID = h.HallID
    ///       JOIN Theaters t ON h.TheaterID = t.TheaterID
    ///       LEFT JOIN Booking_Ticket bt ON sh.ShowID = bt.ShowID
    ///       LEFT JOIN Tickets tk ON bt.TicketID = tk.TicketID
    ///       LEFT JOIN Booking bk ON bt.BookingID = bk.BookingID
    ///       LEFT JOIN Payments p ON bk.PaymentID = p.PaymentID
    ///       WHERE m.MovieID = :movieId
    ///       AND p.PaymentStatus = 'Paid'
    ///       GROUP BY t.TheaterID, t.Name, t.City, t.Address, h.HallID, h.HallName, h.Capacity
    ///       ORDER BY OccupancyPercentage DESC
    ///   ) WHERE ROWNUM <= 3
    /// </summary>
    public class OccupancyViewModel
    {
        public int? SelectedMovieId { get; set; }
        public List<Movie> AllMovies { get; set; } = new();
        public Movie? SelectedMovie { get; set; }
        public List<OccupancyDetail> TopTheaters { get; set; } = new();
    }

    public class OccupancyDetail
    {
        public int TheaterID { get; set; }
        public string TheaterName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int HallID { get; set; }
        public string HallName { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int PaidTickets { get; set; }
        public decimal OccupancyPercentage { get; set; }
    }
}
