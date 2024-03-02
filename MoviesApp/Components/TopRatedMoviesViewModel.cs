using Movies.Entities;

namespace MoviesApp.Components
{
    public class TopRatedMoviesViewModel
    {
        public List<Movie> Movies { get; set; }

        public int LowestRating { get; set; }

        public int NumberOfMoviesToDisplay { get; set; }
    }
}
