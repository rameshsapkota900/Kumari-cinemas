namespace Kumari_cinemas.Models.ViewModels
{
    public class UserTicketViewModel
    {
        public User? SelectedUser { get; set; }
        public List<User> AllUsers { get; set; } = new();
        public List<UserTicketDetail> Tickets { get; set; } = new();
        public int? SelectedUserId { get; set; }
    }

    public class UserTicketDetail
    {
        public int TicketID { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string SeatNumber { get; set; } = string.Empty;
        public int BookingID { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime ShowDate { get; set; }
        public string ShowTime { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
    }
}
