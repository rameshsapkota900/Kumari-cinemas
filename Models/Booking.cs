namespace Kumari_cinemas.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int PaymentID { get; set; }
        public int UserID { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
