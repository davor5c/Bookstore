using ConsoleDump;
using Rhetos;
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
            using (var scope = new ProcessContainer(FindRhetosServerFolder()).CreateTransactionScopeContainer())
            {
                var context = scope.Resolve<Common.ExecutionContext>();
                var repository = context.Repository;

                // See usage examples on Rhetos wiki:
                // https://github.com/Rhetos/Rhetos/wiki/Using-the-Domain-Object-Model

                repository.Bookstore.Book.Query().Take(3).ToList().Dump();
                repository.Bookstore.Book.Load().Take(3).Dump();

                //scope.CommitChanges(); // Database transaction is rolled back by default.
            }
        }

        /// <summary>
        /// This utility is not executed within the main application, so we need to locate it first.
        /// </summary>
        private static string FindRhetosServerFolder()
        {
            string rhetosServerSubfolder = @"src\Bookstore.Service";

            var startingFolder = new DirectoryInfo(Environment.CurrentDirectory);
            var folder = startingFolder;
            while (!Directory.Exists(Path.Combine(folder.FullName, rhetosServerSubfolder)))
            {
                if (folder.Parent == null)
                    throw new ArgumentException($"Cannot find the Rhetos server folder '{rhetosServerSubfolder}' in '{startingFolder}' or any of its parent folders.");

                folder = folder.Parent;
            }

            return Path.Combine(folder.FullName, rhetosServerSubfolder);
        }
    }
}
