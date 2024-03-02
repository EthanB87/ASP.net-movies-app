using Movies.Entities;

namespace MoviesApp.Models
{
    public class MovieDetailsViewModel : AbstractBaseViewModel
    {
        // A reference to the new review potentially submitted via a POST
        public Review? NewReview { get; set; }
    }
}
