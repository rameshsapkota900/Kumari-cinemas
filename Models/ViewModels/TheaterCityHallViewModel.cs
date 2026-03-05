namespace Kumari_cinemas.Models.ViewModels
{
    /// <summary>
    /// ViewModel for the TheaterCityHall Details page.
    /// Combines Theaters and Halls data for the combined CRUD view.
    /// </summary>
    public class TheaterCityHallViewModel
    {
        public List<Theater> Theaters { get; set; } = new();
        public List<Hall> Halls { get; set; } = new();
    }
}
