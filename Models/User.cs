namespace Kumari_cinemas.Models
{
    /// <summary>
    /// Represents the USERS table in the KumariCenimasMGMT Oracle Database.
    /// UserID NUMBER(6) PK, Username VARCHAR2(30), Password VARCHAR2(30), FullName VARCHAR2(50),
    /// Email VARCHAR2(50) UNIQUE NOT NULL, Address VARCHAR2(50), Phone VARCHAR2(15), RegistrationDate DATE
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
