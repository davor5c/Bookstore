using Autofac;
using Rhetos;
using Rhetos.Logging;
using Rhetos.Utilities;
using System;
using System.IO;
using System.Threading;

namespace Bookstore.Service.Test.Tools
{
    /// <summary>
    /// Helper class that manages Dependency Injection container for unit tests.
    /// The container can be customized for each unit test scope.
    /// </summary>
    public static class TestScope
    {
        /// <summary>
        /// Creates a thread-safe lifetime scope DI container (service provider)
        /// to isolate unit of work with a <b>separate database transaction</b>.
        /// To commit changes to database, call <see cref="UnitOfWorkScope.CommitAndClose"/> at the end of the 'using' block.
        /// </summary>
        /// <remarks>
        /// Use helper methods in <see cref="TestScopeContainerBuilderExtensions"/> to configuring components
        /// from the <paramref name="registerCustomComponents"/> delegate.
        /// </remarks>
        public static UnitOfWorkScope Create(Action<ContainerBuilder> registerCustomComponents = null)
        {
            ConsoleLogger.MinLevel = EventType.Info; // Use EventType.Trace for more detailed log.
            return _rhetosHost.CreateScope(registerCustomComponents);
        }

        /// <summary>
        /// Reusing a single shared static DI container between tests, to reduce initialization time for each test.
        /// Each test should create a child scope with <see cref="TestScope.Create"/> method to start a 'using' block.
        /// </summary>
        private static readonly RhetosHost _rhetosHost = RhetosHost.FindBuilder(Path.GetFullPath(@"..\..\..\..\..\src\Bookstore.Service\bin\Debug\net5.0\Bookstore.Service.dll")).Build();
    }
}
