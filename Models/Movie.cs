namespace Kumari_cinemas.Models
{
    /// <summary>
    /// Represents the MOVIES table in the KumariCenimasMGMT Oracle Database.
    /// MovieID NUMBER(5) PK, Title VARCHAR2(50), Duration NUMBER(3), Language VARCHAR2(20), Genre VARCHAR2(20), ReleaseDate DATE
    /// </summary>
    public class Movie
    {
        public int MovieID { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
    }
}
