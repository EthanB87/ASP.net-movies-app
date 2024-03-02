using Microsoft.EntityFrameworkCore;
using Movies.Entities;
using Movies.Services;
using MoviesApp.DataAccess;

namespace MoviesApp.Services
{
    public class MovieManager : IMovieManager
    {
        public MovieManager(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public ICollection<Movie> GetAllMovies()
        {
            var movies = GetBaseQuery()
                .OrderBy(m => m.Name).ToList();

            return movies;
        }

        public List<Movie> GetMoviesByGenreId(string genreId)
        {
            return GetBaseQuery()
                    .Where(m => m.GenreId == genreId)
                    .OrderBy(m => m.Name)
                    .ToList();
        }

        public List<Movie> GetMoviesByLowestRating(int lowestRating)
        {
            return GetBaseQuery()
                    .Where(m => m.Reviews.Average(r => r.Rating).GetValueOrDefault() >= lowestRating)
                    .OrderBy(m => m.Name)
                    .ToList();
        }


        public List<Genre> GetGenres()
        {
            return _movieDbContext.Genres.OrderBy(g => g.Name).ToList();
        }

        public Genre? GetGenreById(string genreId)
        {
            return _movieDbContext.Genres.Find(genreId);
        }


        // Add a new movie and returns its int ID:
        public int AddNewMovie(Movie movie)
        {
            _movieDbContext.Movies.Add(movie);
            _movieDbContext.SaveChanges();
            return movie.MovieId;
        }

        public Movie? GetMovieById(int id)
        {
            return GetBaseQuery()
                    .Where(m => m.MovieId == id)
                    .FirstOrDefault();
        }

        public void UpdateMovie(Movie movie)
        {
            _movieDbContext.Movies.Update(movie);
            _movieDbContext.SaveChanges();
        }

        public void DeleteMovieById(int id)
        {
            var movie = _movieDbContext.Movies.Find(id);
            _movieDbContext.Movies.Remove(movie);
            _movieDbContext.SaveChanges();
        }

        private IQueryable<Movie> GetBaseQuery()
        {
            return _movieDbContext.Movies
                .Include(m => m.Genre)
                .Include(m => m.Reviews)
                .Include(m => m.Castings).ThenInclude(c => c.Actor);
        }

        private MovieDbContext _movieDbContext;
    }
}
