using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;

namespace Bookstore.Service
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            //Uncomment the code below to setup NLog for Dependency injection
            //Such a setup for NLog by default will use the nlog.config file which is included as an example inide this project.
            //This is an example of how to keep the old Rhetos logging funcionality.
            //hostBuilder.UseNLog();

            return hostBuilder;
        }
    }
}
 