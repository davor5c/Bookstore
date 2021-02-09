using Autofac;
using Bookstore.Service.Test.Tools;
using Common.Queryable;
using DemoRowPermissions2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhetos;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Service.Test
{
    [TestClass]
    public class RowPermisionsTest
    {
        [TestMethod]
        public void DocumentRowPermissions()
        {
            string testUserName = "TestUser_" + Guid.NewGuid();

            using (var scope = TestScope.Create(builder => builder
                .ConfigureApplicationUser(testUserName))) // Overrides user authentication with custom user info, see the code inside ConfigureApplicationUser.
            {
                var context = scope.Resolve<Common.ExecutionContext>();
                var repository = scope.Resolve<Common.DomRepository>();
                var rpRepository = repository.DemoRowPermissions2;

                // Adding the account for the current test user.
                repository.Common.Principal.Insert(new Common.Principal { Name = testUserName });

                // Just checking if the authentication override works (see ConfigureApplicationUser above).
                // 'context.UserInfo' returns the authenticated used for the current operation.
                Assert.AreEqual(testUserName, context.UserInfo.UserName);

                //==========================================================================
                // Inserting the test data.
                // Note that direct calls to repository methods will bypass row permissions.
                // Only end-user requests are automatically checked for permissions.

                var region1 = new Region { Name = "region1" };
                rpRepository.Region.Insert(new[] { region1 });

                var division1 = new Division { Name = "division1", RegionID = region1.ID };
                var division2 = new Division { Name = "division2" };
                var division3 = new Division { Name = "division3" };
                rpRepository.Division.Insert(new[] { division1, division2, division3 });

                var employee = new Employee { UserName = testUserName, DivisionID = division3.ID }; // Current user works in division3.
                rpRepository.Employee.Insert(new[] { employee });

                var supervise = new RegionSupervisor { EmployeeID = employee.ID, RegionID = region1.ID }; // Supervises region1 (division1).
                rpRepository.RegionSupervisor.Insert(supervise);
                
                var doc1 = new Document { Title = "doc1", DivisionID = division1.ID }; // The user can access doc1, because it's in the region that employee supervises.
                var doc2 = new Document { Title = "doc2", DivisionID = division2.ID }; // The user cannot access doc2.
                var doc3 = new Document { Title = "doc3", DivisionID = division3.ID }; // The user can access doc3, because it's in the same division.
                var newDocuments = new[] { doc1, doc2, doc3 };
                rpRepository.Document.Insert(newDocuments);

                var documentIds = newDocuments.Select(document => document.ID).ToList();
                var queryDocuments = rpRepository.Document.Query(documentIds);

                //==========================================================================
                // Testing the row permissions for the *current* user (set by ConfigureApplicationUser above).

                var canRead = rpRepository.Document.Filter(queryDocuments, new Common.RowPermissionsReadItems());
                var canWrite = rpRepository.Document.Filter(queryDocuments, new Common.RowPermissionsWriteItems());

                string report = $"Can read: {TestUtility.DumpSorted(canRead, document => document.Title)}." +
                    $" Can write: {TestUtility.DumpSorted(canWrite, document => document.Title)}.";

                Assert.AreEqual("Can read: doc1, doc3. Can write: doc3.", report);
            }
        }
    }
}
