using Bookstore.Service.Test.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhetos.Dom.DefaultConcepts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Service.Test
{
    [TestClass]
    public class DeactivateOnDeleteTest
    {
        [TestMethod]
        public void SimpleDeactivateOnDeleteTest()
        {
            using (var scope = TestScope.Create())
            {
                var repository = scope.Resolve<Common.DomRepository>();

                var book = new Book { Title = Guid.NewGuid().ToString() };
                repository.Bookstore.Book.Insert(book);
                var deleteBooks = new[] { book };
                repository.Bookstore.Book.Save(null, null, deleteBooks, checkUserPermissions: true);
                // The above line calls 'Save' method, instead of a simpler 'Delete' method, to provide more control over the parameters.
                // As convention, the checkUserPermissions parameter is true for operations that are directly called from web API.
                // DeactivateOnDelete is implemented to override only those delete requests, while calling directly Delete method
                // internally will actually delete the records (checkUserPermissions is false by default).

                var deactivatedBook = repository.Bookstore.Book.Load(x => x.ID == book.ID).FirstOrDefault();
                Assert.IsNotNull(deactivatedBook, "The record should not be deleted from the database.");
                Assert.IsFalse(deactivatedBook.Active.Value, "The active flag should be set to false when deleting the record.");
            }
        }
    }
}
