namespace Kumari_cinemas.Models
{
    public class Hall
    {
        public int HallID { get; set; }
        public string HallName { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int TheaterID { get; set; }
        public string? TheaterName { get; set; }
        public string? TheaterCity { get; set; }
    }
}
