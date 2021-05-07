﻿using ConsoleDump;
using Rhetos;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.Logging;
using Rhetos.Utilities;
using System.IO;
using System.Linq;

namespace Bookstore.Playground
{
    static class Program
    {
        static void Main(string[] args)
        {
            ConsoleLogger.MinLevel = EventType.Info; // Use EventType.Trace for more detailed log.
            string rhetosHostAssemblyPath = Path.GetFullPath(@"..\..\..\..\..\src\Bookstore.Service\bin\Debug\net5.0\Bookstore.Service.dll");
            var rhetosHost = RhetosHost.Find(rhetosHostAssemblyPath);
            using (var scope = rhetosHost.CreateScope())
            {
                var context = scope.Resolve<Common.ExecutionContext>();
                var repository = context.Repository;

                // See usage examples on Rhetos wiki:
                // https://github.com/Rhetos/Rhetos/wiki/Using-the-Domain-Object-Model

                repository.Bookstore.Book.Load().Take(3).Dump("Book.Load() returns simple objects");
                repository.Bookstore.Book.Query().Take(3).ToList().Dump("Book.Query() returns queryable objects with navigation properties");

                //scope.CommitAndClose(); // Database transaction is rolled back by default.
            }
        }
    }
}
