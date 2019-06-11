using Bookstore.ServerDom.Test.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhetos;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.TestCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bookstore.ServerDom.Test
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
    }
}
