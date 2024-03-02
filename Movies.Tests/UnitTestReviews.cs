using Movies.Entities;

namespace Movies.Tests
{
    public class UnitTestReviews
    {
        [Fact]
        public void TestAverageRatings()
        {
            // Arrange:
            Movie m1 = new Movie() { 
                Name = "The Matrix",
                Year = 1999,
                Reviews = new List<Review>()
            };

            m1.Reviews.Add(new Review() { Rating = 3, Comments = "Meh, it was ok"});
            m1.Reviews.Add(new Review() { Rating = 5, Comments = "A masterpiece!" });

            // Act:
            double avgReview = m1.Reviews.Average(r => r.Rating).GetValueOrDefault();

            // Assert:
            Assert.Equal(4.0, avgReview);
        }
    }
}