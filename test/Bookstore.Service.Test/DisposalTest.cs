using Bookstore.Service.Test.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhetos;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.TestCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bookstore.Service.Test
{
    [TestClass]
    public class DisposalTest
    {
        [TestMethod]
        public void ImportantBookExplanation()
        {
            using (var rhetos = new BookstoreRhetos())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var book = new Book { Title = "Very important book" };
                repository.Bookstore.Book.Insert(book);

                var disposal1 = new Disposal { BookID = book.ID, Explanation = "long explanation" + new string('!', 100) };
                repository.Bookstore.Disposal.Insert(disposal1);

                var disposal2 = new Disposal { BookID = book.ID, Explanation = "short explanation" };
                TestUtility.ShouldFail<UserException>(
                    () => repository.Bookstore.Disposal.Insert(disposal2),
                    "important", "50 characters");
            }
        }

        [TestMethod]
        public void HighRating()
        {
            using (var rhetos = new BookstoreRhetos())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var book1 = new Book { Title = "Simple book" };
                var book2 = new Book { Title = "Super great book" };
                repository.Bookstore.Book.Insert(book1, book2);

                var ratings = repository.Bookstore.BookRating.Query(new[] { book1.ID, book2.ID })
                    .OrderBy(r => r.Base.Code)
                    .ToSimple().ToList();
                Assert.AreEqual("0.00, 150.00", TestUtility.Dump(ratings, r => r.Rating?.ToString("0.00", CultureInfo.InvariantCulture)));

                var disposal1 = new Disposal { BookID = book1.ID, Explanation = "damaged" };
                repository.Bookstore.Disposal.Insert(disposal1);

                var disposal2 = new Disposal { BookID = book2.ID, Explanation = "damaged" };
                TestUtility.ShouldFail<UserException>(
                    () => repository.Bookstore.Disposal.Insert(disposal2),
                    "rating above 100");
            }
        }

        [TestMethod]
        public void UncertainWords()
        {
            using (var rhetos = new BookstoreRhetos())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                // Removing old data to make this test independent.
                var old = repository.Bookstore.UncertainWord.Load();
                repository.Bookstore.UncertainWord.Delete(old);

                var uncertain = new[] { "sometimes", "maybe", "someone" };
                repository.Bookstore.UncertainWord.Insert(
                    uncertain.Select(word => new UncertainWord { Word = word }));

                var book = new Book { Title = "Some book title" };
                repository.Bookstore.Book.Insert(book);

                var disposal1 = new Disposal { BookID = book.ID, Explanation = "It was damaged" };
                repository.Bookstore.Disposal.Insert(disposal1);

                var disposal2 = new Disposal { BookID = book.ID, Explanation = "Maybe it was damaged" };
                TestUtility.ShouldFail<UserException>(
                    () => repository.Bookstore.Disposal.Insert(disposal2),
                    @"The explanation ""Maybe it w..."" should not contain word ""maybe"". Book: Some book title.");
            }
        }

        [TestMethod]
        public void ExplanationTooLong()
        {
            using (var rhetos = new BookstoreRhetos())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var book = new Book { Title = "Some book" };
                repository.Bookstore.Book.Insert(book);

                TestUtility.ShouldFail<UserException>(
                    () => repository.Bookstore.Disposal.Insert(
                        new Disposal { BookID = book.ID, Explanation = "explanation" + new string('x', 500) }),
                    "Explanation", "longer then", "500");
            }
        }

        [TestMethod]
        public void ExplanationSpecialCharacters()
        {
            using (var rhetos = new BookstoreRhetos())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var book = new Book { Title = "Some book" };
                repository.Bookstore.Book.Insert(book);

                TestUtility.ShouldFail<UserException>(
                    () => repository.Bookstore.Disposal.Insert(
                        new Disposal { BookID = book.ID, Explanation = "explanation#" }),
                    book.Title, "contains character");
            }
        }
    }
}