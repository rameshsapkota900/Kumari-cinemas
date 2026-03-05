namespace Kumari_cinemas.Models
{
    public class TicketPrice
    {
        public int TicketPriceID { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
