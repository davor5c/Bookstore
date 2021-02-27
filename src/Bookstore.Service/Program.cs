using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rhetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Service
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        /// <summary>
        /// Provides the runtime configuration for Rhetos CLI utilities:
        /// configuration settings and system components registration.
        /// </summary>
        public static IRhetosHostBuilder CreateRhetosHostBuilder()
        {
            // Extract web app configuration.
            var host = CreateHostBuilder(null).Build();
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            // Create RhetosHostBuilder and configure it.
            var rhetosHostBuilder = new RhetosHostBuilder();
            Startup.ConfigureRhetosHostBuilder(rhetosHostBuilder, configuration);
            return rhetosHostBuilder;

        }
    }
}
