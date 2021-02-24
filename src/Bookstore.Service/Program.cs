using Autofac;
using Rhetos;
using Rhetos.Logging;
using Rhetos.Security;
using Rhetos.Utilities;

namespace Bookstore.Service
{
    public static class Program
    {
        public static void Main()
        {
        }

        /// <summary>
        /// Provides basic runtime infrastructure for Rhetos framework: configuration settings and system components registration.
        /// </summary>
        public static IRhetosHostBuilder CreateRhetosHostBuilder()
        {
            return new RhetosHostBuilder()
                .ConfigureRhetosHostDefaults()
                .ConfigureConfiguration(builder => builder
                    .AddJsonFile("rhetos-app.settings.json")
                    .AddJsonFile("rhetos-app.local.settings.json"))
                .ConfigureContainer(builder =>
                {
                    // Configuring standard Rhetos system services to work with unit tests:
                    builder.RegisterType<ProcessUserInfo>().As<IUserInfo>();
                    builder.RegisterType<ConsoleLogProvider>().As<ILogProvider>();
                    // Registering custom components for Bookstore application:
                    builder.RegisterType<Bookstore.SmtpMailSender>().As<Bookstore.IMailSender>(); // Application uses SMTP implementation for sending mails. The registration will be overridden in unit tests by fake component.
                    builder.Register(context => context.Resolve<IConfiguration>().GetOptions<Bookstore.MailOptions>()).SingleInstance(); // Standard pattern for registering an options class.
                });
        }
    }
}
