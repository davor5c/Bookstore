using Autofac;
using Rhetos.Configuration.Autofac;
using Rhetos.Logging;
using Rhetos.Security;
using Rhetos.TestCommon;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Service.Test.Tools
{
    public class BookstoreRhetos : RhetosTestContainer
    {
        public BookstoreRhetos(bool commitChanges = false)
            : base(commitChanges, FindRhetosServerFolder())
        {
            ConsoleLogger.MinLevel = EventType.Info; // Use "Trace" for more detailed log.
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

        /// <summary>
        /// Reports all entries for the internal Rhetos system log to the given list of strings.
        /// This method should be called before any object is resolved from the container.
        /// </summary>
        public void AddLogMonitor(List<string> log, EventType minLevel = EventType.Trace)
        {
            InitializeSession += builder =>
                builder.RegisterInstance(new ConsoleLogProvider((eventType, eventName, message) =>
                {
                    log.Add("[" + eventType + "] " + (eventName != null ? (eventName + ": ") : "") + message());
                }))
                .As<ILogProvider>();
        }
        
        /// <summary>
        /// Overrides the default user name.
        /// </summary>
        public void SetCurrentUser(string username)
        {
            InitializeSession += builder =>
                builder.RegisterInstance(new TestUserInfo(username))
                    .As<IUserInfo>();
        }
    }
}
