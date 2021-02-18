﻿using ConsoleDump;
using Rhetos;
using Rhetos.Logging;
using Rhetos.Utilities;
using System.IO;
using System.Linq;

namespace Bookstore.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleLogger.MinLevel = EventType.Info; // Use EventType.Trace for more detailed log.

            string rhetosHostAssemblyPath = @"..\..\..\..\..\src\Bookstore.Service\bin\Debug\net5.0\Bookstore.Service.dll";
            var rhetosHost = RhetosHost.FindBuilder(rhetosHostAssemblyPath).Build();

            using (var scope = rhetosHost.CreateScope())
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
    }
}
