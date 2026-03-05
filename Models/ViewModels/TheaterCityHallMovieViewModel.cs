namespace Kumari_cinemas.Models.ViewModels
{
    public class TheaterCityHallMovieViewModel
    {
        public int? SelectedHallId { get; set; }
        public List<Hall> AllHalls { get; set; } = new();
        public TheaterCityHallInfo? TheaterCityHallInfo { get; set; }
        public List<MovieShowDetail> MovieShows { get; set; } = new();
    }

    public class TheaterCityHallInfo
    {
        public string TheaterName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string HallName { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }

    public class MovieShowDetail
    {
        public int MovieID { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int ShowID { get; set; }
        public DateTime ShowDate { get; set; }
        public string ShowTime { get; set; } = string.Empty;
        public string ShowStatus { get; set; } = string.Empty;
    }
}
