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
                new Bookstore_Book { Title = "abc", AuthorID = people[0].ID },
                new Bookstore_Book { Title = "abc foreign", AuthorID = people[1].ID },
                new Bookstore_Book { Title = "super abc", AuthorID = people[2].ID },
                new Bookstore_Book { Title = "super abc foreign", AuthorID = people[3].ID },
            };

            foreach (var book in books)
            {
                book.ID = Guid.NewGuid();
                if (book.Title.Contains("foreign"))
                    book.Extension_ForeignBook = new Bookstore_ForeignBook { ID = book.ID };
            }

            var booksIds = books.Select(b => b.ID).ToList();

            var ratings = RatingSystem.ComputeRating(booksIds, books.AsQueryable(), people.AsQueryable());

            Assert.AreEqual(
                "0.00, 1.00, 101.00, 121.00",
                string.Join(", ", ratings.Select(r => r.Rating.Value.ToString("f2", CultureInfo.InvariantCulture))));
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
