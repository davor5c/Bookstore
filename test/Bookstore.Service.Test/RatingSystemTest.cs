using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bookstore.Service.Test
{
    [TestClass]
    public class RatingSystemTest
    {
        [TestMethod]
        public void SimpleRatings()
        {
            var tests = new[]
            {
                new RatingInput { BookId = Guid.NewGuid(), Title = "abc", IsForeign = false },
                new RatingInput { BookId = Guid.NewGuid(), Title = "abc", IsForeign = true },
                new RatingInput { BookId = Guid.NewGuid(), Title = "super abc", IsForeign = false },
                new RatingInput { BookId = Guid.NewGuid(), Title = "super abc", IsForeign = true },
            };

            var ratingSystem = new RatingSystem();
            var ratings = ratingSystem.ComputeRating(tests);

            Assert.AreEqual(
                "0.00, 0.00, 100.00, 120.00",
                string.Join(", ", ratings.Select(r => r.Value.ToString("f2", CultureInfo.InvariantCulture))));
        }

        [TestMethod]
        public void UnknownTitle()
        {
            var bookWithoutTitle = new RatingInput
            {
                BookId = Guid.NewGuid(),
                Title = null,
                IsForeign = false
            };

            var ratingSystem = new RatingSystem();
            var result = ratingSystem.ComputeRating(new[] { bookWithoutTitle }).Single();

            Assert.AreEqual(bookWithoutTitle.BookId, result.BookId);
            Assert.AreEqual(0, result.Value);
        }

        [TestMethod]
        public void NoBooks()
        {
            var ratingSystem = new RatingSystem();
            var results = ratingSystem.ComputeRating(Array.Empty<RatingInput>());
            Assert.AreEqual(0, results.Count());
        }
    }
}
