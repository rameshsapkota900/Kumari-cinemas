namespace Kumari_cinemas.Models
{
    /// <summary>
    /// Represents the SHOWS table in the KumariCenimasMGMT Oracle Database.
    /// ShowID NUMBER(5) PK, ShowDate DATE, ShowTime VARCHAR2(10), ShowStatus VARCHAR2(15),
    /// MovieID NUMBER(5) FK, HallID NUMBER(5) FK
    /// </summary>
    public class Show
    {
        public int ShowID { get; set; }
        public DateTime ShowDate { get; set; }
        public string ShowTime { get; set; } = string.Empty;
        public string ShowStatus { get; set; } = string.Empty;
        public int MovieID { get; set; }
        public int HallID { get; set; }

        // Navigation display properties (from JOIN)
        public string? MovieTitle { get; set; }
        public string? HallName { get; set; }
        public string? TheaterName { get; set; }
    }
}
