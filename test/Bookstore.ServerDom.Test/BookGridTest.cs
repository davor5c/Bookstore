using System;
using System.Collections.Generic;
using System.Linq;
using Bookstore.ServerDom.Test.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhetos;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.TestCommon;

namespace Bookstore.ServerDom.Test
{
    [TestClass]
    public class BookGridTest
    {
        [TestMethod]
        public void AlternativeSource()
        {
            using (var rhetos = new BookstoreRhetos())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var wish1 = new Bookstore.WishList { BookTitle = "New book 1 " + RandomString() };
                var wish2 = new Bookstore.WishList { BookTitle = "New book 2 " + RandomString(), HighPriority = true };
                repository.Bookstore.WishList.Insert(wish1, wish2);

                var regularBooks = repository.Bookstore.BookGrid.Query().Select(book => book.Title).ToList();
                Assert.IsFalse(regularBooks.Contains(wish1.BookTitle), "Regular books should not contain the items from a with list.");
                Assert.IsFalse(regularBooks.Contains(wish2.BookTitle), "Regular books should not contain the items from a with list.");

                // Check that the wanted books returns the books from the alternative source (WishList):
                var wantedBooks = repository.Bookstore.BookGrid.Query(new Bookstore.WantedBooks())
                    .Where(book => book.ID == wish1.ID || book.ID == wish2.ID)
                    .ToSimple().ToList();
                // NOTE: If the above line fails with an exception, make sure that the Query 'Bookstore.WantedBooks' implementation
                // returns *all* properties of the Bookstore_BookGrid. For example, if a new property is added to the grid, the EF mapping might fail.
                Assert.AreEqual($"{wish1.BookTitle}, {wish2.BookTitle}", TestUtility.DumpSorted(wantedBooks, book => book.Title));

                // Test the HighPriorityOnly filter parameter:
                var wantedBooksHighPriority = repository.Bookstore.BookGrid.Query(new Bookstore.WantedBooks { HighPriorityOnly = true })
                    .Where(book => book.ID == wish1.ID || book.ID == wish2.ID)
                    .ToSimple().ToList();
                Assert.AreEqual($"{wish2.BookTitle}", TestUtility.DumpSorted(wantedBooksHighPriority, book => book.Title));
            }
        }

        /// <summary>
        /// Use this method to simplify the data isolation between the test runs.
        /// </summary>
        private static string RandomString()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
