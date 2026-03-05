namespace Kumari_cinemas.Models
{
    public class Seat
    {
        public int SeatID { get; set; }
        public int HallID { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
