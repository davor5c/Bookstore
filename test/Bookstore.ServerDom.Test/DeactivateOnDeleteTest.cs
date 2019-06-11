using Bookstore.ServerDom.Test.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhetos;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.ServerDom.Test
{
    [TestClass]
    public class DeactivateOnDeleteTest
    {
        [TestMethod]
        public void SimpleDeactivateOnDeleteTest()
        {
            using (var rhetos = new BookstoreRhetos())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();

                var book = new Book { Title = Guid.NewGuid().ToString() };
                repository.Bookstore.Book.Insert(book);
                repository.Bookstore.Book.Delete(book);

                var deactivatedBook = repository.Bookstore.Book.Load(x => x.ID == book.ID).FirstOrDefault();
                Assert.IsNotNull(deactivatedBook, "The record should not be deleted from the database.");
                Assert.IsFalse(deactivatedBook.Active.Value, "The active flag should be set to false when deleting the record.");
            }
        }
    }
}
