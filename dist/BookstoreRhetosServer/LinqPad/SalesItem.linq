<Query Kind="Program">
  <Reference Relative="..\bin\EntityFramework.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\EntityFramework.dll</Reference>
  <Reference Relative="..\bin\EntityFramework.SqlServer.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\EntityFramework.SqlServer.dll</Reference>
  <Reference Relative="..\bin\NLog.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\NLog.dll</Reference>
  <Reference Relative="..\bin\Oracle.ManagedDataAccess.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Oracle.ManagedDataAccess.dll</Reference>
  <Reference Relative="..\..\..\..\..\Rhetos\Source\Rhetos\bin\Plugins\Rhetos.AspNetFormsAuth.dll">C:\My Projects\Rhetos\Source\Rhetos\bin\Plugins\Rhetos.AspNetFormsAuth.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Configuration.Autofac.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Configuration.Autofac.dll</Reference>
  <Reference Relative="..\bin\Plugins\Rhetos.Dom.DefaultConcepts.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Dom.DefaultConcepts.dll</Reference>
  <Reference Relative="..\bin\Plugins\Rhetos.Dom.DefaultConcepts.Interfaces.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Dom.DefaultConcepts.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Dom.Interfaces.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Dom.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Plugins\Rhetos.Dsl.DefaultConcepts.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Dsl.DefaultConcepts.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Dsl.Interfaces.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Dsl.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Interfaces.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Logging.Interfaces.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Logging.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Persistence.Interfaces.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Persistence.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Plugins\Rhetos.Processing.DefaultCommands.Interfaces.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Processing.DefaultCommands.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Processing.Interfaces.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Processing.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Security.Interfaces.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Security.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Utilities.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Utilities.dll</Reference>
  <Reference Relative="..\bin\Generated\ServerDom.Model.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Generated\ServerDom.Model.dll</Reference>
  <Reference Relative="..\bin\Generated\ServerDom.Orm.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Generated\ServerDom.Orm.dll</Reference>
  <Reference Relative="..\bin\Generated\ServerDom.Repositories.dll">C:\My Projects\RhetosPackages\Bookstore\dist\BookstoreRhetosServer\bin\Generated\ServerDom.Repositories.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.AccountManagement.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.dll</Reference>
  <Namespace>Oracle.ManagedDataAccess.Client</Namespace>
  <Namespace>Rhetos.Configuration.Autofac</Namespace>
  <Namespace>Rhetos.Dom</Namespace>
  <Namespace>Rhetos.Dom.DefaultConcepts</Namespace>
  <Namespace>Rhetos.Dsl</Namespace>
  <Namespace>Rhetos.Dsl.DefaultConcepts</Namespace>
  <Namespace>Rhetos.Logging</Namespace>
  <Namespace>Rhetos.Persistence</Namespace>
  <Namespace>Rhetos.Security</Namespace>
  <Namespace>Rhetos.Utilities</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Data.Entity</Namespace>
  <Namespace>System.DirectoryServices</Namespace>
  <Namespace>System.DirectoryServices.AccountManagement</Namespace>
  <Namespace>System.IO</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Reflection</Namespace>
  <Namespace>System.Runtime.Serialization.Json</Namespace>
  <Namespace>System.Text</Namespace>
  <Namespace>System.Xml</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
    ConsoleLogger.MinLevel = EventType.Info; // Use "Trace" for more details log.
    var rhetosServerPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..");
    Directory.SetCurrentDirectory(rhetosServerPath);
    using (var container = new RhetosTestContainer(commitChanges: false)) // Use this parameter to COMMIT or ROLLBACK the data changes.
    {
        var context = container.Resolve<Common.ExecutionContext>();
        var repository = context.Repository;

		repository.Bookstore.Book.Insert(new Bookstore.Book { Title = "abc", NumberOfPages = 700, Price = 123, Code = "B+" });
		
		repository.Bookstore.Food.Insert(new Bookstore.Food { Price = 456, Code = "F+++", Description = "tasty" });
		
		var salesItems = repository.Bookstore.SalesItem.Load().Dump();

		repository.Bookstore.SalesItemComment.Insert(
			salesItems.SelectMany(si =>
				new[] { 1, 2, 3 }.Select(x =>
					new Bookstore.SalesItemComment
					{
						SalesItemID = si.ID,
						Comment = $"{x}-{si.Description}"
					})));
						
		repository.Bookstore.SalesItemComment.Load().Dump();
    }
}