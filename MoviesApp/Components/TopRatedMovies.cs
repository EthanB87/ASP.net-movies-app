using Microsoft.AspNetCore.Mvc;

using Movies.Services;

namespace MoviesApp.Components
{
    public class TopRatedMovies : ViewComponent
    {
        public TopRatedMovies(IMovieManager movieManager)
        {
            _movieManager = movieManager;
        }

        public IViewComponentResult Invoke(int lowestRating, int numberOfMoviesToDisplay)
        {
            var movies = _movieManager.GetMoviesByLowestRating(lowestRating);

            TopRatedMoviesViewModel topRatedMoviesViewModel = new TopRatedMoviesViewModel() { 
                Movies = movies,
                NumberOfMoviesToDisplay = numberOfMoviesToDisplay,
                LowestRating = lowestRating
            };

            return View(topRatedMoviesViewModel);
        }

        private IMovieManager _movieManager;
    }
}
