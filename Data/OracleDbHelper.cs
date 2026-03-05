using Oracle.ManagedDataAccess.Client;
using System.Data;
using Kumari_cinemas.Models;
using Kumari_cinemas.Models.ViewModels;

namespace Kumari_cinemas.Data
{
    public class OracleDbHelper
    {
        private readonly string _connectionString;

        public OracleDbHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        private OracleConnection GetConnection()
        {
            return new OracleConnection(_connectionString);
        }

        // ======================================================================
        // THEATERS - CRUD
        // Schema: TheaterID (PK IDENTITY), Name, City, Address
        // ======================================================================

        public List<Theater> GetAllTheaters()
        {
            var list = new List<Theater>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("SELECT TheaterID, Name, City, Address FROM Theaters ORDER BY TheaterID", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Theater
                {
                    TheaterID = Convert.ToInt32(reader["TheaterID"]),
                    Name = reader["Name"].ToString()!,
                    City = reader["City"].ToString()!,
                    Address = reader["Address"].ToString()!
                });
            }
            return list;
        }

        public Theater? GetTheaterById(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("SELECT TheaterID, Name, City, Address FROM Theaters WHERE TheaterID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Theater
                {
                    TheaterID = Convert.ToInt32(reader["TheaterID"]),
                    Name = reader["Name"].ToString()!,
                    City = reader["City"].ToString()!,
                    Address = reader["Address"].ToString()!
                };
            }
            return null;
        }

        public void InsertTheater(Theater t)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "INSERT INTO Theaters (Name, City, Address) VALUES (:name, :city, :addr)", conn);
            cmd.Parameters.Add(new OracleParameter("name", t.Name));
            cmd.Parameters.Add(new OracleParameter("city", t.City));
            cmd.Parameters.Add(new OracleParameter("addr", t.Address));
            cmd.ExecuteNonQuery();
        }

        public void UpdateTheater(Theater t)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "UPDATE Theaters SET Name = :name, City = :city, Address = :addr WHERE TheaterID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("name", t.Name));
            cmd.Parameters.Add(new OracleParameter("city", t.City));
            cmd.Parameters.Add(new OracleParameter("addr", t.Address));
            cmd.Parameters.Add(new OracleParameter("id", t.TheaterID));
            cmd.ExecuteNonQuery();
        }

        public void DeleteTheater(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("DELETE FROM Theaters WHERE TheaterID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            cmd.ExecuteNonQuery();
        }

        // ======================================================================
        // HALLS - CRUD
        // Schema: HallID (PK IDENTITY), TheaterID (FK), HallName, Capacity
        // ======================================================================

        public List<Hall> GetAllHalls()
        {
            var list = new List<Hall>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"SELECT h.HallID, h.HallName, h.Capacity, h.TheaterID,
                         t.Name AS TheaterName, t.City AS TheaterCity
                  FROM Halls h
                  JOIN Theaters t ON h.TheaterID = t.TheaterID
                  ORDER BY h.HallID", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Hall
                {
                    HallID = Convert.ToInt32(reader["HallID"]),
                    HallName = reader["HallName"].ToString()!,
                    Capacity = Convert.ToInt32(reader["Capacity"]),
                    TheaterID = Convert.ToInt32(reader["TheaterID"]),
                    TheaterName = reader["TheaterName"].ToString(),
                    TheaterCity = reader["TheaterCity"].ToString()
                });
            }
            return list;
        }

        public Hall? GetHallById(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"SELECT h.HallID, h.HallName, h.Capacity, h.TheaterID,
                         t.Name AS TheaterName, t.City AS TheaterCity
                  FROM Halls h
                  JOIN Theaters t ON h.TheaterID = t.TheaterID
                  WHERE h.HallID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Hall
                {
                    HallID = Convert.ToInt32(reader["HallID"]),
                    HallName = reader["HallName"].ToString()!,
                    Capacity = Convert.ToInt32(reader["Capacity"]),
                    TheaterID = Convert.ToInt32(reader["TheaterID"]),
                    TheaterName = reader["TheaterName"].ToString(),
                    TheaterCity = reader["TheaterCity"].ToString()
                };
            }
            return null;
        }

        public void InsertHall(Hall h)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "INSERT INTO Halls (HallName, Capacity, TheaterID) VALUES (:name, :cap, :tid)", conn);
            cmd.Parameters.Add(new OracleParameter("name", h.HallName));
            cmd.Parameters.Add(new OracleParameter("cap", h.Capacity));
            cmd.Parameters.Add(new OracleParameter("tid", h.TheaterID));
            cmd.ExecuteNonQuery();
        }

        public void UpdateHall(Hall h)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "UPDATE Halls SET HallName = :name, Capacity = :cap, TheaterID = :tid WHERE HallID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("name", h.HallName));
            cmd.Parameters.Add(new OracleParameter("cap", h.Capacity));
            cmd.Parameters.Add(new OracleParameter("tid", h.TheaterID));
            cmd.Parameters.Add(new OracleParameter("id", h.HallID));
            cmd.ExecuteNonQuery();
        }

        public void DeleteHall(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("DELETE FROM Halls WHERE HallID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            cmd.ExecuteNonQuery();
        }

        // ======================================================================
        // MOVIES - CRUD
        // Schema: MovieID (PK IDENTITY), Title, Duration, Language, Genre, ReleaseDate
        // ======================================================================

        public List<Movie> GetAllMovies()
        {
            var list = new List<Movie>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "SELECT MovieID, Title, Duration, Language, Genre, ReleaseDate FROM Movies ORDER BY MovieID", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Movie
                {
                    MovieID = Convert.ToInt32(reader["MovieID"]),
                    Title = reader["Title"].ToString()!,
                    Duration = Convert.ToInt32(reader["Duration"]),
                    Language = reader["Language"].ToString()!,
                    Genre = reader["Genre"].ToString()!,
                    ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"])
                });
            }
            return list;
        }

        public Movie? GetMovieById(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "SELECT MovieID, Title, Duration, Language, Genre, ReleaseDate FROM Movies WHERE MovieID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Movie
                {
                    MovieID = Convert.ToInt32(reader["MovieID"]),
                    Title = reader["Title"].ToString()!,
                    Duration = Convert.ToInt32(reader["Duration"]),
                    Language = reader["Language"].ToString()!,
                    Genre = reader["Genre"].ToString()!,
                    ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"])
                };
            }
            return null;
        }

        public void InsertMovie(Movie m)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "INSERT INTO Movies (Title, Duration, Language, Genre, ReleaseDate) VALUES (:title, :dur, :lang, :genre, :rdate)", conn);
            cmd.Parameters.Add(new OracleParameter("title", m.Title));
            cmd.Parameters.Add(new OracleParameter("dur", m.Duration));
            cmd.Parameters.Add(new OracleParameter("lang", m.Language));
            cmd.Parameters.Add(new OracleParameter("genre", m.Genre));
            cmd.Parameters.Add(new OracleParameter("rdate", m.ReleaseDate));
            cmd.ExecuteNonQuery();
        }

        public void UpdateMovie(Movie m)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "UPDATE Movies SET Title = :title, Duration = :dur, Language = :lang, Genre = :genre, ReleaseDate = :rdate WHERE MovieID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("title", m.Title));
            cmd.Parameters.Add(new OracleParameter("dur", m.Duration));
            cmd.Parameters.Add(new OracleParameter("lang", m.Language));
            cmd.Parameters.Add(new OracleParameter("genre", m.Genre));
            cmd.Parameters.Add(new OracleParameter("rdate", m.ReleaseDate));
            cmd.Parameters.Add(new OracleParameter("id", m.MovieID));
            cmd.ExecuteNonQuery();
        }

        public void DeleteMovie(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("DELETE FROM Movies WHERE MovieID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            cmd.ExecuteNonQuery();
        }

        // ======================================================================
        // SHOWS - CRUD
        // Schema: ShowID (PK IDENTITY), ShowDate, ShowTime, ShowStatus, MovieID (FK), HallID (FK)
        // ======================================================================

        public List<Show> GetAllShows()
        {
            var list = new List<Show>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"SELECT sh.ShowID, sh.ShowDate, sh.ShowTime, sh.ShowStatus,
                         sh.MovieID, sh.HallID, m.Title AS MovieTitle,
                         h.HallName, t.Name AS TheaterName
                  FROM Shows sh
                  JOIN Movies m ON sh.MovieID = m.MovieID
                  JOIN Halls h ON sh.HallID = h.HallID
                  JOIN Theaters t ON h.TheaterID = t.TheaterID
                  ORDER BY sh.ShowID", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Show
                {
                    ShowID = Convert.ToInt32(reader["ShowID"]),
                    ShowDate = Convert.ToDateTime(reader["ShowDate"]),
                    ShowTime = reader["ShowTime"].ToString()!,
                    ShowStatus = reader["ShowStatus"].ToString()!,
                    MovieID = Convert.ToInt32(reader["MovieID"]),
                    HallID = Convert.ToInt32(reader["HallID"]),
                    MovieTitle = reader["MovieTitle"].ToString(),
                    HallName = reader["HallName"].ToString(),
                    TheaterName = reader["TheaterName"].ToString()
                });
            }
            return list;
        }

        public Show? GetShowById(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"SELECT sh.ShowID, sh.ShowDate, sh.ShowTime, sh.ShowStatus,
                         sh.MovieID, sh.HallID, m.Title AS MovieTitle,
                         h.HallName, t.Name AS TheaterName
                  FROM Shows sh
                  JOIN Movies m ON sh.MovieID = m.MovieID
                  JOIN Halls h ON sh.HallID = h.HallID
                  JOIN Theaters t ON h.TheaterID = t.TheaterID
                  WHERE sh.ShowID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Show
                {
                    ShowID = Convert.ToInt32(reader["ShowID"]),
                    ShowDate = Convert.ToDateTime(reader["ShowDate"]),
                    ShowTime = reader["ShowTime"].ToString()!,
                    ShowStatus = reader["ShowStatus"].ToString()!,
                    MovieID = Convert.ToInt32(reader["MovieID"]),
                    HallID = Convert.ToInt32(reader["HallID"]),
                    MovieTitle = reader["MovieTitle"].ToString(),
                    HallName = reader["HallName"].ToString(),
                    TheaterName = reader["TheaterName"].ToString()
                };
            }
            return null;
        }

        public void InsertShow(Show s)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "INSERT INTO Shows (ShowDate, ShowTime, ShowStatus, MovieID, HallID) VALUES (:sdate, :stime, :status, :mid, :hid)", conn);
            cmd.Parameters.Add(new OracleParameter("sdate", s.ShowDate));
            cmd.Parameters.Add(new OracleParameter("stime", s.ShowTime));
            cmd.Parameters.Add(new OracleParameter("status", s.ShowStatus));
            cmd.Parameters.Add(new OracleParameter("mid", s.MovieID));
            cmd.Parameters.Add(new OracleParameter("hid", s.HallID));
            cmd.ExecuteNonQuery();
        }

        public void UpdateShow(Show s)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "UPDATE Shows SET ShowDate = :sdate, ShowTime = :stime, ShowStatus = :status, MovieID = :mid, HallID = :hid WHERE ShowID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("sdate", s.ShowDate));
            cmd.Parameters.Add(new OracleParameter("stime", s.ShowTime));
            cmd.Parameters.Add(new OracleParameter("status", s.ShowStatus));
            cmd.Parameters.Add(new OracleParameter("mid", s.MovieID));
            cmd.Parameters.Add(new OracleParameter("hid", s.HallID));
            cmd.Parameters.Add(new OracleParameter("id", s.ShowID));
            cmd.ExecuteNonQuery();
        }

        public void DeleteShow(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("DELETE FROM Shows WHERE ShowID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            cmd.ExecuteNonQuery();
        }

        // ======================================================================
        // USERS - CRUD
        // Schema: UserID (PK IDENTITY), Username, Password, FullName, Email, Address, Phone, RegistrationDate
        // ======================================================================

        public List<User> GetAllUsers()
        {
            var list = new List<User>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "SELECT UserID, Username, Password, FullName, Email, Address, Phone, RegistrationDate FROM Users ORDER BY UserID", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new User
                {
                    UserID = Convert.ToInt32(reader["UserID"]),
                    Username = reader["Username"].ToString()!,
                    Password = reader["Password"].ToString()!,
                    FullName = reader["FullName"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString(),
                    Phone = reader["Phone"] == DBNull.Value ? null : reader["Phone"].ToString(),
                    RegistrationDate = reader["RegistrationDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["RegistrationDate"])
                });
            }
            return list;
        }

        public User? GetUserById(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "SELECT UserID, Username, Password, FullName, Email, Address, Phone, RegistrationDate FROM Users WHERE UserID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserID = Convert.ToInt32(reader["UserID"]),
                    Username = reader["Username"].ToString()!,
                    Password = reader["Password"].ToString()!,
                    FullName = reader["FullName"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString(),
                    Phone = reader["Phone"] == DBNull.Value ? null : reader["Phone"].ToString(),
                    RegistrationDate = reader["RegistrationDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["RegistrationDate"])
                };
            }
            return null;
        }

        public void InsertUser(User u)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"INSERT INTO Users (Username, Password, FullName, Email, Address, Phone, RegistrationDate)
                  VALUES (:uname, :pwd, :fname, :email, :addr, :phone, SYSDATE)", conn);
            cmd.Parameters.Add(new OracleParameter("uname", u.Username));
            cmd.Parameters.Add(new OracleParameter("pwd", u.Password));
            cmd.Parameters.Add(new OracleParameter("fname", u.FullName));
            cmd.Parameters.Add(new OracleParameter("email", u.Email));
            cmd.Parameters.Add(new OracleParameter("addr", (object?)u.Address ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("phone", (object?)u.Phone ?? DBNull.Value));
            cmd.ExecuteNonQuery();
        }

        public void UpdateUser(User u)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"UPDATE Users SET Username = :uname, Password = :pwd, FullName = :fname, Email = :email,
                  Address = :addr, Phone = :phone WHERE UserID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("uname", u.Username));
            cmd.Parameters.Add(new OracleParameter("pwd", u.Password));
            cmd.Parameters.Add(new OracleParameter("fname", u.FullName));
            cmd.Parameters.Add(new OracleParameter("email", u.Email));
            cmd.Parameters.Add(new OracleParameter("addr", (object?)u.Address ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("phone", (object?)u.Phone ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("id", u.UserID));
            cmd.ExecuteNonQuery();
        }

        public void DeleteUser(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("DELETE FROM Users WHERE UserID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            cmd.ExecuteNonQuery();
        }

        // ======================================================================
        // TICKETS - CRUD
        // Schema: TicketID (PK IDENTITY), TicketPriceID (FK), CancellationID (FK nullable), SeatID (FK), IssueDate
        // ======================================================================

        public List<Ticket> GetAllTickets()
        {
            var list = new List<Ticket>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"SELECT tk.TicketID, tk.IssueDate,
                         tk.TicketPriceID, tk.CancellationID, tk.SeatID,
                         tp.Price, tp.Category,
                         s.SeatNumber, s.Status AS SeatStatus
                  FROM Tickets tk
                  JOIN TicketPrices tp ON tk.TicketPriceID = tp.TicketPriceID
                  JOIN Seats s ON tk.SeatID = s.SeatID
                  ORDER BY tk.TicketID", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Ticket
                {
                    TicketID = Convert.ToInt32(reader["TicketID"]),
                    IssueDate = reader["IssueDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["IssueDate"]),
                    TicketPriceID = Convert.ToInt32(reader["TicketPriceID"]),
                    CancellationID = reader["CancellationID"] == DBNull.Value ? null : Convert.ToInt32(reader["CancellationID"]),
                    SeatID = Convert.ToInt32(reader["SeatID"]),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Category = reader["Category"].ToString(),
                    SeatNumber = reader["SeatNumber"].ToString(),
                    SeatStatus = reader["SeatStatus"].ToString()
                });
            }
            return list;
        }

        public Ticket? GetTicketById(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"SELECT tk.TicketID, tk.IssueDate,
                         tk.TicketPriceID, tk.CancellationID, tk.SeatID,
                         tp.Price, tp.Category,
                         s.SeatNumber, s.Status AS SeatStatus
                  FROM Tickets tk
                  JOIN TicketPrices tp ON tk.TicketPriceID = tp.TicketPriceID
                  JOIN Seats s ON tk.SeatID = s.SeatID
                  WHERE tk.TicketID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Ticket
                {
                    TicketID = Convert.ToInt32(reader["TicketID"]),
                    IssueDate = reader["IssueDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["IssueDate"]),
                    TicketPriceID = Convert.ToInt32(reader["TicketPriceID"]),
                    CancellationID = reader["CancellationID"] == DBNull.Value ? null : Convert.ToInt32(reader["CancellationID"]),
                    SeatID = Convert.ToInt32(reader["SeatID"]),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Category = reader["Category"].ToString(),
                    SeatNumber = reader["SeatNumber"].ToString(),
                    SeatStatus = reader["SeatStatus"].ToString()
                };
            }
            return null;
        }

        public void InsertTicket(Ticket t)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"INSERT INTO Tickets (TicketPriceID, CancellationID, SeatID, IssueDate)
                  VALUES (:tpid, :cid, :sid, SYSDATE)", conn);
            cmd.Parameters.Add(new OracleParameter("tpid", t.TicketPriceID));
            cmd.Parameters.Add(new OracleParameter("cid", (object?)t.CancellationID ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("sid", t.SeatID));
            cmd.ExecuteNonQuery();
        }

        public void UpdateTicket(Ticket t)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"UPDATE Tickets SET TicketPriceID = :tpid, CancellationID = :cid, SeatID = :sid WHERE TicketID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("tpid", t.TicketPriceID));
            cmd.Parameters.Add(new OracleParameter("cid", (object?)t.CancellationID ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("sid", t.SeatID));
            cmd.Parameters.Add(new OracleParameter("id", t.TicketID));
            cmd.ExecuteNonQuery();
        }

        public void DeleteTicket(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("DELETE FROM Tickets WHERE TicketID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            cmd.ExecuteNonQuery();
        }

        // Helper: Get all TicketPrices for dropdown
        // Schema: TicketPriceID (PK IDENTITY), Price, Category
        public List<TicketPrice> GetAllTicketPrices()
        {
            var list = new List<TicketPrice>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("SELECT TicketPriceID, Price, Category FROM TicketPrices ORDER BY TicketPriceID", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new TicketPrice
                {
                    TicketPriceID = Convert.ToInt32(reader["TicketPriceID"]),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Category = reader["Category"].ToString()!
                });
            }
            return list;
        }

        // Helper: Get all Seats for dropdown
        // Schema: SeatID (PK IDENTITY), HallID (FK), SeatNumber, Status
        public List<Seat> GetAllSeats()
        {
            var list = new List<Seat>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("SELECT SeatID, HallID, SeatNumber, Status FROM Seats ORDER BY SeatID", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Seat
                {
                    SeatID = Convert.ToInt32(reader["SeatID"]),
                    HallID = Convert.ToInt32(reader["HallID"]),
                    SeatNumber = reader["SeatNumber"].ToString()!,
                    Status = reader["Status"].ToString()!
                });
            }
            return list;
        }

        // Helper: Get all Cancellations for dropdown
        // Schema: CancellationID (PK IDENTITY), Reason, CancelDate
        public List<Cancellation> GetAllCancellations()
        {
            var list = new List<Cancellation>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("SELECT CancellationID, Reason, CancelDate FROM Cancellations ORDER BY CancellationID", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Cancellation
                {
                    CancellationID = Convert.ToInt32(reader["CancellationID"]),
                    Reason = reader["Reason"] == DBNull.Value ? null : reader["Reason"].ToString(),
                    CancelDate = reader["CancelDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["CancelDate"])
                });
            }
            return list;
        }

        // ======================================================================
        // TICKET PRICES - Full CRUD
        // Schema: TicketPriceID (PK IDENTITY), Price, Category
        // ======================================================================

        public TicketPrice? GetTicketPriceById(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("SELECT TicketPriceID, Price, Category FROM TicketPrices WHERE TicketPriceID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TicketPrice
                {
                    TicketPriceID = Convert.ToInt32(reader["TicketPriceID"]),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Category = reader["Category"].ToString()!
                };
            }
            return null;
        }

        public void InsertTicketPrice(TicketPrice tp)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "INSERT INTO TicketPrices (Price, Category) VALUES (:price, :cat)", conn);
            cmd.Parameters.Add(new OracleParameter("price", tp.Price));
            cmd.Parameters.Add(new OracleParameter("cat", tp.Category));
            cmd.ExecuteNonQuery();
        }

        public void UpdateTicketPrice(TicketPrice tp)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "UPDATE TicketPrices SET Price = :price, Category = :cat WHERE TicketPriceID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("price", tp.Price));
            cmd.Parameters.Add(new OracleParameter("cat", tp.Category));
            cmd.Parameters.Add(new OracleParameter("id", tp.TicketPriceID));
            cmd.ExecuteNonQuery();
        }

        public void DeleteTicketPrice(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("DELETE FROM TicketPrices WHERE TicketPriceID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            cmd.ExecuteNonQuery();
        }

        // ======================================================================
        // SEATS - Full CRUD
        // Schema: SeatID (PK IDENTITY), HallID (FK), SeatNumber, Status
        // ======================================================================

        public Seat? GetSeatById(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"SELECT s.SeatID, s.HallID, s.SeatNumber, s.Status, h.HallName
                  FROM Seats s JOIN Halls h ON s.HallID = h.HallID
                  WHERE s.SeatID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Seat
                {
                    SeatID = Convert.ToInt32(reader["SeatID"]),
                    HallID = Convert.ToInt32(reader["HallID"]),
                    SeatNumber = reader["SeatNumber"].ToString()!,
                    Status = reader["Status"].ToString()!
                };
            }
            return null;
        }

        public void InsertSeat(Seat s)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "INSERT INTO Seats (HallID, SeatNumber, Status) VALUES (:hid, :snum, :status)", conn);
            cmd.Parameters.Add(new OracleParameter("hid", s.HallID));
            cmd.Parameters.Add(new OracleParameter("snum", s.SeatNumber));
            cmd.Parameters.Add(new OracleParameter("status", s.Status));
            cmd.ExecuteNonQuery();
        }

        public void UpdateSeat(Seat s)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "UPDATE Seats SET HallID = :hid, SeatNumber = :snum, Status = :status WHERE SeatID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("hid", s.HallID));
            cmd.Parameters.Add(new OracleParameter("snum", s.SeatNumber));
            cmd.Parameters.Add(new OracleParameter("status", s.Status));
            cmd.Parameters.Add(new OracleParameter("id", s.SeatID));
            cmd.ExecuteNonQuery();
        }

        public void DeleteSeat(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("DELETE FROM Seats WHERE SeatID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            cmd.ExecuteNonQuery();
        }

        // ======================================================================
        // CANCELLATIONS - Full CRUD
        // Schema: CancellationID (PK IDENTITY), Reason, CancelDate
        // ======================================================================

        public Cancellation? GetCancellationById(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("SELECT CancellationID, Reason, CancelDate FROM Cancellations WHERE CancellationID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Cancellation
                {
                    CancellationID = Convert.ToInt32(reader["CancellationID"]),
                    Reason = reader["Reason"] == DBNull.Value ? null : reader["Reason"].ToString(),
                    CancelDate = reader["CancelDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["CancelDate"])
                };
            }
            return null;
        }

        public void InsertCancellation(Cancellation c)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "INSERT INTO Cancellations (Reason, CancelDate) VALUES (:reason, :cdate)", conn);
            cmd.Parameters.Add(new OracleParameter("reason", (object?)c.Reason ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("cdate", (object?)c.CancelDate ?? DBNull.Value));
            cmd.ExecuteNonQuery();
        }

        public void UpdateCancellation(Cancellation c)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                "UPDATE Cancellations SET Reason = :reason, CancelDate = :cdate WHERE CancellationID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("reason", (object?)c.Reason ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("cdate", (object?)c.CancelDate ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("id", c.CancellationID));
            cmd.ExecuteNonQuery();
        }

        public void DeleteCancellation(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand("DELETE FROM Cancellations WHERE CancellationID = :id", conn);
            cmd.Parameters.Add(new OracleParameter("id", id));
            cmd.ExecuteNonQuery();
        }

        // ======================================================================
        // COMPLEX FORM 1: User Ticket
        // ======================================================================

        public UserTicketViewModel GetUserTickets(int userId)
        {
            var vm = new UserTicketViewModel
            {
                AllUsers = GetAllUsers(),
                SelectedUserId = userId,
                SelectedUser = GetUserById(userId),
                Tickets = new List<UserTicketDetail>()
            };

            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"SELECT tk.TicketID, tk.IssueDate,
                         tp.Price, tp.Category,
                         s.SeatNumber,
                         b.BookingID, b.BookingDate,
                         sh.ShowDate, sh.ShowTime, m.Title AS MovieTitle
                  FROM Users u
                  JOIN Booking b ON u.UserID = b.UserID
                  JOIN Booking_Ticket bt ON b.BookingID = bt.BookingID
                  JOIN Tickets tk ON bt.TicketID = tk.TicketID
                  JOIN Shows sh ON bt.ShowID = sh.ShowID
                  JOIN Movies m ON sh.MovieID = m.MovieID
                  JOIN Seats s ON tk.SeatID = s.SeatID
                  JOIN TicketPrices tp ON tk.TicketPriceID = tp.TicketPriceID
                  WHERE u.UserID = :userId
                  AND tk.IssueDate >= ADD_MONTHS(SYSDATE, -6)
                  ORDER BY tk.IssueDate DESC", conn);
            cmd.Parameters.Add(new OracleParameter("userId", userId));
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                vm.Tickets.Add(new UserTicketDetail
                {
                    TicketID = Convert.ToInt32(reader["TicketID"]),
                    IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Category = reader["Category"].ToString()!,
                    SeatNumber = reader["SeatNumber"].ToString()!,
                    BookingID = Convert.ToInt32(reader["BookingID"]),
                    BookingDate = Convert.ToDateTime(reader["BookingDate"]),
                    ShowDate = Convert.ToDateTime(reader["ShowDate"]),
                    ShowTime = reader["ShowTime"].ToString()!,
                    MovieTitle = reader["MovieTitle"].ToString()!
                });
            }
            return vm;
        }

        // ======================================================================
        // COMPLEX FORM 2: TheaterCityHall Movie
        // ======================================================================

        public TheaterCityHallMovieViewModel GetTheaterCityHallMovies(int hallId)
        {
            var vm = new TheaterCityHallMovieViewModel
            {
                AllHalls = GetAllHalls(),
                SelectedHallId = hallId,
                MovieShows = new List<MovieShowDetail>()
            };

            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"SELECT t.TheaterID, t.Name AS TheaterName, t.City, t.Address,
                         h.HallID, h.HallName, h.Capacity,
                         m.MovieID, m.Title, m.Duration, m.Language, m.Genre, m.ReleaseDate,
                         sh.ShowID, sh.ShowDate, sh.ShowTime, sh.ShowStatus
                  FROM Theaters t
                  JOIN Halls h ON t.TheaterID = h.TheaterID
                  JOIN Shows sh ON h.HallID = sh.HallID
                  JOIN Movies m ON sh.MovieID = m.MovieID
                  WHERE h.HallID = :hallId
                  ORDER BY sh.ShowDate, sh.ShowTime", conn);
            cmd.Parameters.Add(new OracleParameter("hallId", hallId));
            using var reader = cmd.ExecuteReader();
            bool first = true;
            while (reader.Read())
            {
                if (first)
                {
                    vm.TheaterCityHallInfo = new TheaterCityHallInfo
                    {
                        TheaterName = reader["TheaterName"].ToString()!,
                        City = reader["City"].ToString()!,
                        Address = reader["Address"].ToString()!,
                        HallName = reader["HallName"].ToString()!,
                        Capacity = Convert.ToInt32(reader["Capacity"])
                    };
                    first = false;
                }
                vm.MovieShows.Add(new MovieShowDetail
                {
                    MovieID = Convert.ToInt32(reader["MovieID"]),
                    Title = reader["Title"].ToString()!,
                    Duration = Convert.ToInt32(reader["Duration"]),
                    Language = reader["Language"].ToString()!,
                    Genre = reader["Genre"].ToString()!,
                    ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]),
                    ShowID = Convert.ToInt32(reader["ShowID"]),
                    ShowDate = Convert.ToDateTime(reader["ShowDate"]),
                    ShowTime = reader["ShowTime"].ToString()!,
                    ShowStatus = reader["ShowStatus"].ToString()!
                });
            }
            return vm;
        }

        // ======================================================================
        // COMPLEX FORM 3: Occupancy Performer
        // ======================================================================

        public OccupancyViewModel GetOccupancyPerformer(int movieId)
        {
            var vm = new OccupancyViewModel
            {
                AllMovies = GetAllMovies(),
                SelectedMovieId = movieId,
                SelectedMovie = GetMovieById(movieId),
                TopTheaters = new List<OccupancyDetail>()
            };

            using var conn = GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(
                @"SELECT * FROM (
                      SELECT t.TheaterID, t.Name AS TheaterName, t.City, t.Address,
                             h.HallID, h.HallName, h.Capacity,
                             COUNT(tk.TicketID) AS PaidTickets,
                             ROUND(COUNT(tk.TicketID) * 100.0 / h.Capacity, 2) AS OccupancyPct
                      FROM Movies m
                      JOIN Shows sh ON m.MovieID = sh.MovieID
                      JOIN Halls h ON sh.HallID = h.HallID
                      JOIN Theaters t ON h.TheaterID = t.TheaterID
                      LEFT JOIN Booking_Ticket bt ON sh.ShowID = bt.ShowID
                      LEFT JOIN Tickets tk ON bt.TicketID = tk.TicketID
                      LEFT JOIN Booking bk ON bt.BookingID = bk.BookingID
                      LEFT JOIN Payments p ON bk.PaymentID = p.PaymentID
                      WHERE m.MovieID = :movieId
                      AND p.Status = 'Paid'
                      GROUP BY t.TheaterID, t.Name, t.City, t.Address, h.HallID, h.HallName, h.Capacity
                      ORDER BY OccupancyPct DESC
                  ) WHERE ROWNUM <= 3", conn);
            cmd.Parameters.Add(new OracleParameter("movieId", movieId));
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                vm.TopTheaters.Add(new OccupancyDetail
                {
                    TheaterID = Convert.ToInt32(reader["TheaterID"]),
                    TheaterName = reader["TheaterName"].ToString()!,
                    City = reader["City"].ToString()!,
                    Address = reader["Address"].ToString()!,
                    HallID = Convert.ToInt32(reader["HallID"]),
                    HallName = reader["HallName"].ToString()!,
                    Capacity = Convert.ToInt32(reader["Capacity"]),
                    PaidTickets = Convert.ToInt32(reader["PaidTickets"]),
                    OccupancyPercentage = Convert.ToDecimal(reader["OccupancyPct"])
                });
            }
            return vm;
        }

        // ======================================================================
        // DASHBOARD
        // ======================================================================

        public DashboardViewModel GetDashboardData()
        {
            var vm = new DashboardViewModel();
            using var conn = GetConnection();
            conn.Open();

            vm.TotalMovies = GetScalarInt(conn, "SELECT COUNT(*) FROM Movies");
            vm.TotalTheaters = GetScalarInt(conn, "SELECT COUNT(*) FROM Theaters");
            vm.TotalHalls = GetScalarInt(conn, "SELECT COUNT(*) FROM Halls");
            vm.TotalShows = GetScalarInt(conn, "SELECT COUNT(*) FROM Shows");
            vm.TotalUsers = GetScalarInt(conn, "SELECT COUNT(*) FROM Users");
            vm.TotalBookings = GetScalarInt(conn, "SELECT COUNT(*) FROM Booking");
            vm.TotalTickets = GetScalarInt(conn, "SELECT COUNT(*) FROM Tickets");
            vm.TotalRevenue = GetScalarDecimal(conn, "SELECT NVL(SUM(Amount), 0) FROM Payments WHERE Status = 'Paid'");
            vm.ActiveShows = GetScalarInt(conn, "SELECT COUNT(*) FROM Shows WHERE ShowStatus = 'Scheduled'");
            vm.PaidTickets = GetScalarInt(conn, "SELECT COUNT(*) FROM Tickets WHERE CancellationID IS NULL");
            vm.CancelledTickets = GetScalarInt(conn, "SELECT COUNT(*) FROM Tickets WHERE CancellationID IS NOT NULL");

            // Genre distribution
            using (var cmd2 = new OracleCommand("SELECT Genre, COUNT(*) AS Cnt FROM Movies GROUP BY Genre ORDER BY Cnt DESC", conn))
            using (var r2 = cmd2.ExecuteReader())
            {
                while (r2.Read())
                {
                    vm.GenreLabels.Add(r2["Genre"].ToString()!);
                    vm.GenreCounts.Add(Convert.ToInt32(r2["Cnt"]));
                }
            }

            // Bookings per theater
            using (var cmd3 = new OracleCommand(
                @"SELECT t.Name, COUNT(sb.BookingID) AS Cnt
                  FROM Theaters t
                  JOIN Halls h ON t.TheaterID = h.TheaterID
                  JOIN Shows sh ON h.HallID = sh.HallID
                  JOIN Show_Booking sb ON sh.ShowID = sb.ShowID
                  GROUP BY t.Name
                  ORDER BY Cnt DESC", conn))
            using (var r3 = cmd3.ExecuteReader())
            {
                while (r3.Read())
                {
                    vm.TheaterLabels.Add(r3["Name"].ToString()!);
                    vm.TheaterBookingCounts.Add(Convert.ToInt32(r3["Cnt"]));
                }
            }

            // Monthly revenue
            using (var cmd4 = new OracleCommand(
                @"SELECT TO_CHAR(PaymentDate, 'YYYY-MM') AS Mon, SUM(Amount) AS Total
                  FROM Payments
                  WHERE Status = 'Paid' AND PaymentDate IS NOT NULL
                  GROUP BY TO_CHAR(PaymentDate, 'YYYY-MM')
                  ORDER BY Mon", conn))
            using (var r4 = cmd4.ExecuteReader())
            {
                while (r4.Read())
                {
                    vm.MonthLabels.Add(r4["Mon"].ToString()!);
                    vm.MonthlyRevenue.Add(Convert.ToDecimal(r4["Total"]));
                }
            }

            return vm;
        }

        private int GetScalarInt(OracleConnection conn, string sql)
        {
            using var cmd = new OracleCommand(sql, conn);
            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        private decimal GetScalarDecimal(OracleConnection conn, string sql)
        {
            using var cmd = new OracleCommand(sql, conn);
            var result = cmd.ExecuteScalar();
            return Convert.ToDecimal(result);
        }
    }
}
