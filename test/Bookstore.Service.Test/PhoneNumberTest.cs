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
    public class PhoneNumberTest
    {
        [TestMethod]
        public void PhoneNumbersShouldNotContainAlphabetCharactersTest()
        {
            using (var container = BookstoreContainer.CreateTransactionScope())
            {
                var repository = container.Resolve<Common.DomRepository>();

                var person = new Person { Name = Guid.NewGuid().ToString(), MobilePhone = "07700-a00759" };
                TestUtility.ShouldFail<UserException>(() => repository.Bookstore.Person.Insert(person),
                    "Invalid phone number format.");
            }
        }
    }
}
