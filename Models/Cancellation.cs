namespace Kumari_cinemas.Models
{
    public class Cancellation
    {
        public int CancellationID { get; set; }
        public string? Reason { get; set; }
        public DateTime? CancelDate { get; set; }
    }
}
