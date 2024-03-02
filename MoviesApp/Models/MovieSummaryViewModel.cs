using Movies.Entities;

namespace MoviesApp.Models
{
    public class MovieSummaryViewModel : AbstractBaseViewModel
    {
        public int NumberOfReviews { get; set; }

        public string ActorDisplayText { get; set; }
    }
}
