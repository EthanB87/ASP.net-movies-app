using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Movies.Entities;
using Movies.Services;
using MoviesApp.DataAccess;
using MoviesApp.Models;

namespace MoviesApp.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        public MovieController(IMovieManager movieManager, MovieDbContext movieDbContext)
        {
            _movieManager = movieManager;
            _movieDbContext = movieDbContext;
        }

        [HttpGet("/movies")]
        public IActionResult GetAllMovies()
        {
            List<MovieSummaryViewModel> movieSummaries = _movieManager.GetAllMovies()
                .Select(m => new MovieSummaryViewModel() {
                    ActiveMovie = m,
                    NumberOfReviews = m.Reviews.Count,
                    AverageRating = m.Reviews.Average(r => r.Rating).GetValueOrDefault(),
                    ActorDisplayText = GetActorDisplayText(m)
                })
                .ToList();
            
            return View("Items", movieSummaries);
        }

        [HttpGet("/movies/{id}")]
        public IActionResult GetMovieById(int id)
        {
            var movie = _movieManager.GetMovieById(id);

            MovieDetailsViewModel movieDetailsViewModel = new MovieDetailsViewModel() { 
                ActiveMovie = movie,
                AverageRating = movie.Reviews.Average(r => r.Rating).GetValueOrDefault()
            };

            return View("Details", movieDetailsViewModel);
        }

        [HttpGet("/movies/genres/{genreId}")]
        public IActionResult GetMoviesByGenreId(string genreId)
        {
            // first query for the genre:
            var activeGenre = _movieManager.GetGenreById(genreId);

            // and then movies of that genre:
            var moviesByGenre = _movieManager.GetMoviesByGenreId(genreId);

            MoviesByGenreViewModel moviesByGenreViewModel = new MoviesByGenreViewModel()
            {
                Movies = moviesByGenre,
                ActiveGenreName = activeGenre.Name
            };

            return View("ItemsByGenre", moviesByGenreViewModel);
        }

        [HttpGet("/movies/add-request")]
        [Authorize()]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAddMovieRequest()
        {
            MovieViewModel movieViewModel = new MovieViewModel()
            {
                Genres = _movieManager.GetGenres(),
                ActiveMovie = new Movie()
            };

            return View("AddMovie", movieViewModel);
        }

        [HttpPost("/movies")]
        public IActionResult AddNewMovie(MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {
                // it's valid so we want to add the new movie to the DB:
                _movieManager.AddNewMovie(movieViewModel.ActiveMovie);

                TempData["LastActionMessage"] = $"The movie \"{movieViewModel.ActiveMovie.Name}\" ({movieViewModel.ActiveMovie.Year}) was added.";
                return RedirectToAction("GetAllMovies", "Movie");
            }
            else
            {
                // it's invalid so we simply return the movie object
                // to the Edit view again:
                movieViewModel.Genres = _movieManager.GetGenres();
                return View("AddMovie", movieViewModel);
            }
        }

        [HttpPost("/movies/{id}/reviews")]
        public IActionResult AddReviewToMovieById(int id, MovieDetailsViewModel movieDetailsViewModel)
        {
            // retrieve the whole movie:
            var movie = _movieManager.GetMovieById(id);

            movie.Reviews.Add(movieDetailsViewModel.NewReview);

            _movieManager.UpdateMovie(movie);

            // redirect back to the same details view pasing the ID to the URL:
            return RedirectToAction("GetMovieById", "Movie", new { id = id});
        }

        [HttpGet("/movies/{id}/edit-request")]
        public IActionResult GetEditRequestById(int id)
        {
            MovieViewModel movieViewModel = new MovieViewModel()
            {
                Genres = _movieManager.GetGenres(),
                ActiveMovie = _movieManager.GetMovieById(id)
            };

            return View("EditMovie", movieViewModel);
        }

        [HttpPost("/movies/edit-requests")]
        public IActionResult ProcessEditRequest(MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {
                _movieManager.UpdateMovie(movieViewModel.ActiveMovie);

                TempData["LastActionMessage"] = $"The movie \"{movieViewModel.ActiveMovie.Name}\" ({movieViewModel.ActiveMovie.Year}) was updated.";

                return RedirectToAction("GetAllMovies", "Movie");
            }
            else
            {
                movieViewModel.Genres = _movieManager.GetGenres();
                return View("EditMovie", movieViewModel);
            }
        }

        [HttpGet("/movies/{id}/delete-request")]
        public IActionResult GetDeleteRequestById(int id)
        {
            // Find/retrieve/select the movie to edit and then pass it to the view:
            var movie = _movieManager.GetMovieById(id);
            return View("Delete", movie);
        }

        // Here we also improve the way delete works using attr routing:
        [HttpPost("/movies/{id}/delete-requests")]
        public IActionResult ProcessDeleteRequestById(int id)
        {
            var movie = _movieManager.GetMovieById(id);
            _movieManager.DeleteMovieById(id);

            TempData["LastActionMessage"] = $"The movie \"{movie.Name}\" ({movie.Year}) was deleted.";

            return RedirectToAction("GetAllMovies", "Movie");
        }

        // A provate helper method:
        private static string GetActorDisplayText(Movie movie)
        {
            int numActors = movie.Castings.Count;

            if (numActors == 0)
            {
                return "";
            }
            else if (numActors == 1)
            {
                return movie.Castings.ElementAt(0).Actor.FullName;
            }
            else if (numActors == 2)
            {
                return movie.Castings.ElementAt(0).Actor.FullName + 
                    ", " + movie.Castings.ElementAt(1).Actor.FullName;
            }
            else
            {
                return movie.Castings.ElementAt(0).Actor.FullName +
                    ", " + movie.Castings.ElementAt(1).Actor.FullName + "...";
            }
        }

        private IMovieManager _movieManager;
        private MovieDbContext _movieDbContext;
    }
}
