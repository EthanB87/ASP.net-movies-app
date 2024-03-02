using Movies.Entities;

namespace Movies.Services
{
    public interface IMovieManager
    {
        public ICollection<Movie> GetAllMovies();

        public List<Movie> GetMoviesByGenreId(string genreId);

        public List<Movie> GetMoviesByLowestRating(int lowestRating);

        public List<Genre> GetGenres();

        public Genre? GetGenreById(string genreId);

        // Add a new movie and returns its int ID:
        public int AddNewMovie(Movie movie);

        public Movie? GetMovieById(int id);

        public void UpdateMovie(Movie movie);

        public void DeleteMovieById(int id);
    }
}
