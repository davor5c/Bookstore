using Autofac;
using Rhetos;
using Rhetos.Logging;
using Rhetos.TestCommon;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Bookstore.Service.Test.Tools
{
    /// <summary>
    /// Dependency Injection container for Bookstore unit tests.
    /// </summary>
    public static class BookstoreContainer
    {
        private static readonly Lazy<ProcessContainer> _processContainer = new Lazy<ProcessContainer>(
            () => new ProcessContainer(FindBookstoreServiceFolder()), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// This method creates a thread-safe lifetime scope DI container to isolate unit of work in a separate database transaction.
        /// To commit changes to database, call <see cref="TransactionScopeContainer.CommitChanges"/> at the end of the 'using' block.
        /// </summary>
        public static TransactionScopeContainer CreateTransactionScope(Action<ContainerBuilder> registerCustomComponents = null)
        {
            ConsoleLogger.MinLevel = EventType.Info; // Use EventType.Trace for more detailed log.
            return _processContainer.Value.CreateTransactionScopeContainer(registerCustomComponents);
        }

        /// <summary>
        /// Unit tests can be executed at different disk locations depending on whether they are run at the solution or project level, from Visual Studio or another utility.
        /// Therefore, instead of providing a simple relative path, this method searches for the main application location.
        /// </summary>
        private static string FindBookstoreServiceFolder()
        {
            var startingFolder = new DirectoryInfo(Environment.CurrentDirectory);
            string rhetosServerSubfolder = @"src\Bookstore.Service";

            var folder = startingFolder;
            while (!Directory.Exists(Path.Combine(folder.FullName, rhetosServerSubfolder)))
            {
                if (folder.Parent == null)
                    throw new ArgumentException($"Cannot find the Rhetos server folder '{rhetosServerSubfolder}' in '{startingFolder}' or any of its parent folders.");
                folder = folder.Parent;
            }

            return Path.Combine(folder.FullName, rhetosServerSubfolder);
        }

        /// <summary>
        /// Reports all entries from Rhetos system log to the given list of strings.
        /// Pass the created delegate to <see cref="CreateTransactionScope"/> to customize the Dependency Injection container.
        /// </summary>
        public static Action<ContainerBuilder> ConfigureLogMonitor(List<string> log, EventType minLevel = EventType.Trace)
        {
            return builder =>
                builder.RegisterInstance(new ConsoleLogProvider((eventType, eventName, message) =>
                {
                    if (eventType >= minLevel)
                        log.Add("[" + eventType + "] " + (eventName != null ? (eventName + ": ") : "") + message());
                }))
                .As<ILogProvider>();
        }

        /// <summary>
        /// Override the default application user (current process account) for testing.
        /// Pass the created delegate to <see cref="CreateTransactionScope"/> to customize the Dependency Injection container.
        /// </summary>
        public static Action<ContainerBuilder> ConfigureApplicationUser(string username)
        {
            return builder =>
                builder.RegisterInstance(new TestUserInfo(username)).As<IUserInfo>();
        }
    }
}
