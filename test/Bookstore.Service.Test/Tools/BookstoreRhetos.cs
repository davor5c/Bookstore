using Autofac;
using Rhetos.Configuration.Autofac;
using Rhetos.Logging;
using Rhetos.TestCommon;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bookstore.Service.Test.Tools
{
    public class BookstoreRhetos
    {
        private static Lazy<RhetosProcessContainer> _rhetosProcessContainer = new Lazy<RhetosProcessContainer>(() => new RhetosProcessContainer(FindBookstoreServiceFolder), true);

        public static RhetosTransactionScopeContainer GetIocContainer(bool commitChanges = false, Action<ContainerBuilder> configureContainer = null)
        {
            ConsoleLogger.MinLevel = EventType.Info; // Use "Trace" for more detailed log.
            return _rhetosProcessContainer.Value.CreateTransactionScope(commitChanges, configureContainer);
        }

        private static string FindBookstoreServiceFolder()
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

        /// <summary>
        /// Creates a delegate to report all entries for the internal Rhetos system log to the given list of strings.
        /// Pass the created delegate to <see cref="GetIocContainer"/> to customize the Dependency Injection container.
        /// </summary>
        public static Action<ContainerBuilder> CreateLogMonitorDelegate(List<string> log, EventType minLevel = EventType.Trace)
        {
            return builder =>
                builder.RegisterInstance(new ConsoleLogProvider((eventType, eventName, message) =>
                {
                    log.Add("[" + eventType + "] " + (eventName != null ? (eventName + ": ") : "") + message());
                }))
                .As<ILogProvider>();
        }

        /// <summary>
        /// Creates a delegate to override the default user name.
        /// Pass the created delegate to <see cref="GetIocContainer"/> to customize the Dependency Injection container.
        /// </summary>
        public static Action<ContainerBuilder> CreateSetCurrentUserDelegate(string username)
        {
            return builder =>
                builder.RegisterInstance(new TestUserInfo(username))
                    .As<IUserInfo>();
        }
    }
}
