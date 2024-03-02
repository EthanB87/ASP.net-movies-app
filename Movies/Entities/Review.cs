using System.ComponentModel.DataAnnotations;

namespace Movies.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Please enter a rating.")]
        [Range(1, 5, ErrorMessage = "Rating must be btween 1 and 5.")]
        public int? Rating { get; set; }

        public string? Comments { get; set; }

        // FK to the movie being reviewed:
        public int MovieId { get; set; }

        // Nav to Movie:
        public Movie? Movie { get; set; }
    }
}
