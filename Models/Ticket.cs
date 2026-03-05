namespace Kumari_cinemas.Models
{
    public class Ticket
    {
        public int TicketID { get; set; }
        public int TicketPriceID { get; set; }
        public int? CancellationID { get; set; }
        public int SeatID { get; set; }
        public DateTime IssueDate { get; set; }
        // Navigation display properties (from JOIN)
        public decimal? Price { get; set; }
        public string? Category { get; set; }
        public string? SeatNumber { get; set; }
        public string? SeatStatus { get; set; }
    }
}
