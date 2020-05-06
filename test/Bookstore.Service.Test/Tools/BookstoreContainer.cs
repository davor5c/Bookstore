using Autofac;
using Rhetos.Configuration.Autofac;
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
    /// Dependency Injection container for Bookstore application.
    /// </summary>
    public static class BookstoreContainer
    {
        private static readonly Lazy<RhetosProcessContainer> _rhetosProcessContainer = new Lazy<RhetosProcessContainer>(
            () => new RhetosProcessContainer(FindBookstoreServiceFolder()), LazyThreadSafetyMode.ExecutionAndPublication);

        public static RhetosTransactionScopeContainer CreateTransactionScope(Action<ContainerBuilder> configureContainer = null)
        {
            ConsoleLogger.MinLevel = EventType.Info; // Use EventType.Trace for more detailed log.
            return _rhetosProcessContainer.Value.CreateTransactionScope(configureContainer);
        }

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
        public static Action<ContainerBuilder> CreateLogMonitorDelegate(List<string> log, EventType minLevel = EventType.Trace)
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
        public static Action<ContainerBuilder> CreateSetCurrentUserDelegate(string username)
        {
            return builder =>
                builder.RegisterInstance(new TestUserInfo(username)).As<IUserInfo>();
        }
    }
}
