using ConsoleDump;
using Rhetos.Configuration.Autofac;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.Logging;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleLogger.MinLevel = EventType.Info; // Use "Trace" for more details log.
            var rhetosServerPath = @"C:\Bojan\Bookstore\dist\BookstoreRhetosServer";
            Directory.SetCurrentDirectory(rhetosServerPath);
            using (var container = new RhetosTestContainer(commitChanges: false)) // Use this parameter to COMMIT or ROLLBACK the data changes.
            {
                var context = container.Resolve<Common.ExecutionContext>();
                var repository = context.Repository;

                repository.Bookstore.Book.Load().Dump();
                repository.Bookstore.Book.Query().ToList().Dump();
            }
        }
    }
}
