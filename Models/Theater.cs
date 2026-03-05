namespace Kumari_cinemas.Models
{
    /// <summary>
    /// Represents the THEATERS table in the KumariCenimasMGMT Oracle Database.
    /// TheaterID NUMBER(5) PRIMARY KEY, Name VARCHAR2(30), City VARCHAR2(20), Address VARCHAR2(50)
    /// </summary>
    public class Theater
    {
        public int TheaterID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
