using Bookstore.RhetosServer.Test.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhetos;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.RhetosServer.Test
{
    [TestClass]
    public class BookTest
    {
        /// <summary>
        /// BookInfo.NumberOfComments is persisted in a cache table and should be automatically updated
        /// each a comment is added or deleted.
        /// </summary>
        [TestMethod]
        public void AutomaticallyUpdateNumberOfComments()
        {
            using (var rhetos = new BookstoreRhetos())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var book = new Book { Title = Guid.NewGuid().ToString() };
                repository.Bookstore.Book.Insert(book);

                int? readNumberOfComments() => repository.Bookstore.BookInfo
                    .Query(bi => bi.ID == book.ID)
                    .Select(bi => bi.NumberOfComments)
                    .Single();

                Assert.AreEqual(0, readNumberOfComments());

                var c1 = new Comment { BookID = book.ID, Text = "c1" };
                var c2 = new Comment { BookID = book.ID, Text = "c2" };
                var c3 = new Comment { BookID = book.ID, Text = "c3" };

                repository.Bookstore.Comment.Insert(c1);
                Assert.AreEqual(1, readNumberOfComments());

                repository.Bookstore.Comment.Insert(c2, c3);
                Assert.AreEqual(3, readNumberOfComments());

                repository.Bookstore.Comment.Delete(c1);
                Assert.AreEqual(2, readNumberOfComments());

                repository.Bookstore.Comment.Delete(c2, c3);
                Assert.AreEqual(0, readNumberOfComments());
            }
        }

        [TestMethod]
        public void CommonMisspellingValidation()
        {
            using (var rhetos = new BookstoreRhetos())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var book = new Book { Title = "x 'curiousity' y" };

                TestUtility.ShouldFail<UserException>(
                    () => repository.Bookstore.Book.Insert(book),
                    "It is not allowed to enter misspelled word");
            }
        }
    }
}
