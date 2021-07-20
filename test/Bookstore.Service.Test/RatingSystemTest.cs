using System;
using System.Globalization;
using System.Linq;
using Common.Queryable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bookstore.Service.Test
{
    [TestClass]
    public class RatingSystemTest
    {
        [TestMethod]
        public void SimpleRatings()
        {
            var people = new[]
            {
                new Bookstore_Person { Name = "A" },
                new Bookstore_Person { Name = "AA" },
                new Bookstore_Person { Name = "AAA" },
                new Bookstore_Person { Name = "AAAA" },
            };

            foreach (var person in people)
                person.ID = Guid.NewGuid();

            var books = new[]
            {
                NewBook("abc", people[0]),
                NewBook("abc foreign", people[1]),
                NewBook("super abc", people[2]),
                NewBook("super abc foreign", people[3]),
            };

            var booksIds = books.Select(b => b.ID).ToList();

            var ratings = RatingSystem.ComputeRating(booksIds, books.AsQueryable(), people.AsQueryable());

            string report = string.Join(", ", ratings.Select(r => r.Rating.Value.ToString("f2", CultureInfo.InvariantCulture)));
            Assert.AreEqual("0.00, 1.00, 101.00, 121.00", report);
        }

        /// <summary>
        /// Creates a fake *queryable* book.
        /// </summary>
        static Bookstore_Book NewBook(string title, Bookstore_Person author)
        {
            Guid bookId = Guid.NewGuid();
            return new Bookstore_Book
            {
                ID = bookId,
                Title = title,
                AuthorID = author.ID,
                Author = author,
                Extension_ForeignBook = title.Contains("foreign")
                    ? new Bookstore_ForeignBook { ID = bookId }
                    : null
            };
        }

        [TestMethod]
        public void UnknownTitle()
        {
            var bookWithoutTitle = new Bookstore_Book
            {
                ID = Guid.NewGuid(),
                Title = null
            };

            var results = RatingSystem.ComputeRating(
                new[] { bookWithoutTitle.ID },
                booksQuery: new[] { bookWithoutTitle }.AsQueryable(),
                personQuery: new Bookstore_Person[] { }.AsQueryable());

            Assert.AreEqual(bookWithoutTitle.ID, results.Single().ID);
            Assert.AreEqual(0, results.Single().Rating);
        }

        [TestMethod]
        public void NoBooks()
        {
            var results = RatingSystem.ComputeRating(
                Array.Empty<Guid>(),
                Array.Empty<Bookstore_Book>().AsQueryable(),
                Array.Empty<Bookstore_Person>().AsQueryable());
            Assert.AreEqual(0, results.Count());
        }
    }
}
