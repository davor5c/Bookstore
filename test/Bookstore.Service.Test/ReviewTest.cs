using Bookstore.Service.Test.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhetos;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Service.Test
{
    [TestClass]
    public class ReviewTest
    {
        [TestMethod]
        public void DefaultTextFromScore()
        {
            using (var rhetos = BookstoreRhetos.GetIocContainer())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var book = new Bookstore.Book { Title = nameof(DefaultTextFromScore) };
                repository.Bookstore.Book.Insert(book);

                var review1 = new Bookstore.Review { BookID = book.ID, Score = 1 };
                var review2 = new Bookstore.Review { BookID = book.ID, Score = 3, Text = "OK" };
                var review3 = new Bookstore.Review { BookID = book.ID, Score = 5 };
                repository.Bookstore.Review.Insert(review1, review2, review3);

                var reviews = repository.Bookstore.Review.Load(new[] { review1.ID, review2.ID, review3.ID });
                Assert.AreEqual(
                    "1 - I don't like it, 3 - OK, 5 - I like it",
                    TestUtility.DumpSorted(reviews, r => r.Score + " - " + r.Text));
            }
        }

        [TestMethod]
        public void AppendTextIfScoreChanged()
        {
            using (var rhetos = BookstoreRhetos.GetIocContainer())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var book = new Bookstore.Book { Title = nameof(AppendTextIfScoreChanged) };
                repository.Bookstore.Book.Insert(book);

                var reviews = new[]
                {
                    new Bookstore.Review { BookID = book.ID, Score = 2, Text = "A" },
                    new Bookstore.Review { BookID = book.ID, Score = 3, Text = "B" },
                    new Bookstore.Review { BookID = book.ID, Score = 4, Text = "C" },
                };
                repository.Bookstore.Review.Insert(reviews);

                // Reload before editing.
                reviews = repository.Bookstore.Review.Load(reviews.Select(r => r.ID))
                    .OrderBy(r => r.Score).ToArray();
                reviews[0].Score = 3;
                reviews[1].Score = 5;
                repository.Bookstore.Review.Update(reviews);

                Assert.AreEqual(
                    "3 - A (changed from 2 to 3), 4 - C, 5 - B (changed from 3 to 5)",
                    TestUtility.DumpSorted(
                        repository.Bookstore.Review.Query(reviews.Select(r => r.ID)),
                        r => r.Score + " - " + r.Text));
            }
        }

        [TestMethod]
        public void UpdateNumberOfReviews()
        {
            using (var rhetos = BookstoreRhetos.GetIocContainer())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var book = new Bookstore.Book { Title = nameof(UpdateNumberOfReviews) };
                repository.Bookstore.Book.Insert(book);

                int? getNuberOfReviews() => repository.Bookstore.NumberOfReviews
                    .Query(nor => nor.ID == book.ID)
                    .Select(nor => nor.Count)
                    .SingleOrDefault();

                var review1 = new Bookstore.Review { BookID = book.ID, Score = 3 };
                var review2 = new Bookstore.Review { BookID = book.ID, Score = 4 };
                repository.Bookstore.Review.Insert(review1, review2);
                Assert.AreEqual(2, getNuberOfReviews());

                repository.Bookstore.Review.Delete(review1);
                Assert.AreEqual(1, getNuberOfReviews());
            }
        }

        [TestMethod]
        public void DenyChangeOfLockedTitle()
        {
            using (var rhetos = BookstoreRhetos.GetIocContainer())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var book1 = new Bookstore.Book { Title = "book1" };
                var book2 = new Bookstore.Book { Title = "book2 locked" };
                repository.Bookstore.Book.Insert(book1, book2);

                var review1 = new Bookstore.Review { BookID = book1.ID, Score = 3 };
                var review2 = new Bookstore.Review { BookID = book2.ID, Score = 4 };
                repository.Bookstore.Review.Insert(review1, review2);

                review1.Text = "updated text 1";
                repository.Bookstore.Review.Update(review1);

                review1.Score += 1;
                repository.Bookstore.Review.Update(review1);

                review2.Text = "updated text 2";
                repository.Bookstore.Review.Update(review2);

                review2.Score += 1;
                TestUtility.ShouldFail<UserException>(
                    () => repository.Bookstore.Review.Update(review2),
                    "It is not allowed to modify score");
            }
        }
    }
}
