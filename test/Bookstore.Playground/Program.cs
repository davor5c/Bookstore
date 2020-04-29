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
            ConsoleLogger.MinLevel = EventType.Info; // Use EventType.Trace for more detailed log.
            // Set commitChanges parameter to COMMIT or ROLLBACK the data changes.
            using (var container = new RhetosProcessContainer(FindRhetosServerFolder).CreateTransactionScope(false))
            {
                var context = container.Resolve<Common.ExecutionContext>();
                var repository = context.Repository;

                // See usage examples on Rhetos wiki:
                // https://github.com/Rhetos/Rhetos/wiki/Using-the-Domain-Object-Model

                repository.Bookstore.Book.Query().Take(3).ToList().Dump();
                repository.Bookstore.Book.Load().Take(3).Dump();
            }
        }

        private static string FindRhetosServerFolder()
        {
            string rhetosServerSubfolder = @"src\Bookstore.Service";

            var startingFolder = new DirectoryInfo(Environment.CurrentDirectory);
            var folder = startingFolder;
            while (!Directory.Exists(Path.Combine(folder.FullName, rhetosServerSubfolder)))
            {
                if (folder.Parent == null)
                    throw new ApplicationException($"Cannot find the Rhetos server folder '{rhetosServerSubfolder}' in '{startingFolder}' or any of its parent folders.");

                folder = folder.Parent;
            }

            return Path.Combine(folder.FullName, rhetosServerSubfolder);
        }
    }
}
