using Autofac;
using Rhetos.Logging;
using Rhetos.TestCommon;
using Rhetos.Utilities;
using System.Collections.Generic;

namespace Bookstore.Service.Test.Tools
{
    /// <summary>
    /// Helper methods for configuring <see cref="TestScope"/> components in scope of a unit tests.
    /// </summary>
    public static class TestScopeContainerBuilderExtensions
    {
        /// <summary>
        /// Reports all entries from Rhetos system log to the given list of strings.
        /// </summary>
        public static ContainerBuilder ConfigureLogMonitor(this ContainerBuilder builder, List<string> log, EventType minLevel = EventType.Trace)
        {
            builder.RegisterInstance(new ConsoleLogProvider((eventType, eventName, message) =>
                {
                    if (eventType >= minLevel)
                        log.Add("[" + eventType + "] " + (eventName != null ? (eventName + ": ") : "") + message());
                }))
                .As<ILogProvider>();
            return builder;
        }

        /// <summary>
        /// Override the default application user (current process account) for testing.
        /// </summary>
        public static ContainerBuilder ConfigureApplicationUser(this ContainerBuilder builder, string username)
        {
            builder.RegisterInstance(new TestUserInfo(username)).As<IUserInfo>();
            return builder;
        }
    }
}
