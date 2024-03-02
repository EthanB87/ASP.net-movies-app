using Movies.Entities;

namespace MoviesApp.Models
{
    public abstract class AbstractBaseViewModel
    {
        public Movie? ActiveMovie { get; set; }

        public double AverageRating { get; set; }
    }
}
