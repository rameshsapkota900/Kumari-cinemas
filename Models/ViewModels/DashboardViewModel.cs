namespace Kumari_cinemas.Models.ViewModels
{
    /// <summary>
    /// ViewModel for the Homepage Dashboard.
    /// Provides summary statistics for the KumariCinemas system.
    /// </summary>
    public class DashboardViewModel
    {
        public int TotalMovies { get; set; }
        public int TotalTheaters { get; set; }
        public int TotalHalls { get; set; }
        public int TotalShows { get; set; }
        public int TotalUsers { get; set; }
        public int TotalBookings { get; set; }
        public int TotalTickets { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ActiveShows { get; set; }
        public int PaidTickets { get; set; }
        public int CancelledTickets { get; set; }

        // Chart data
        public List<string> GenreLabels { get; set; } = new();
        public List<int> GenreCounts { get; set; } = new();
        public List<string> TheaterLabels { get; set; } = new();
        public List<int> TheaterBookingCounts { get; set; } = new();
        public List<string> MonthLabels { get; set; } = new();
        public List<decimal> MonthlyRevenue { get; set; } = new();
    }
}
