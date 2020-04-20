using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhetos.Dom.DefaultConcepts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.RhetosServer.Test.Tools
{
    [TestClass]
    public static class Profiling
    {
        [AssemblyInitialize]
        public static void StartupTime(TestContext testContext)
        {
            var sw = Stopwatch.StartNew();
            using (var rhetos = new BookstoreRhetos())
            {
                var repository = rhetos.Resolve<Common.DomRepository>();
                var book = new Bookstore.Book { Code = Guid.NewGuid().ToString(), Title = "abc" };
                repository.Bookstore.Book.Insert(book);
            }
            Console.WriteLine($"Rhetos service startup time: {sw.Elapsed}");
        }
    }
}
