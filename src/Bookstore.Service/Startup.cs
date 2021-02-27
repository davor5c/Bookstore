using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Rhetos;
using Rhetos.Logging;
using Rhetos.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestApp", Version = "v1" });
                // Adding Rhetos REST API to Swagger with document name "rhetos".
                c.SwaggerDoc("rhetos", new OpenApiInfo { Title = "Rhetos REST API", Version = "v1" });
            });

            // Using NewtonsoftJson for backward-compatibility with older versions of Rhetos.RestGenerator:
            // legacy Microsoft DateTime serialization and
            // byte[] serialization as JSON array of integers instead of Base64 string.
            services.AddControllers()
                .AddNewtonsoftJson(o =>
                {
                    o.UseMemberCasing();
                    o.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                    o.SerializerSettings.Converters.Add(new Rhetos.Host.AspNet.RestApi.Utilities.ByteArrayConverter());
                });

            // Adding Rhetos to AspNetCore application.
            services.AddRhetos(rhetosHostBuilder => ConfigureRhetosHostBuilder(rhetosHostBuilder, Configuration))
                .UseAspNetCoreIdentityUser()
                .AddRestApi(o =>
                {
                    o.BaseRoute = "rest";
                    o.GroupNameMapper = (conceptInfo, name) => "rhetos"; // OpenAPI document name.
                });

            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            // TODO: Configuring Authentication and Authorization.
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(o => o.Events.OnRedirectToLogin = context =>
            //    {
            //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //        return Task.CompletedTask;
            //    });

            //services.AddAuthorization(a =>
            //{
            //    a.FallbackPolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
            //        .RequireAuthenticatedUser()
            //        .Build();
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/rhetos/swagger.json", "Rhetos Rest Api");
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestApp v1");
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// This setup is extracted to separate public static method so it can be used BOTH from Startup class
        /// and any other code that wishes to recreate RhetosHost specific for this web application
        /// Common use is to call this from Program.CreateRhetosHostBuilder method which is by convention consumed by
        /// Rhetos tools.
        /// </summary>
        public static void ConfigureRhetosHostBuilder(IRhetosHostBuilder rhetosHostBuilder, IConfiguration configuration)
        {
            rhetosHostBuilder
                .ConfigureRhetosHostDefaults()
                .UseBuilderLogProvider(new Rhetos.Host.Net.Logging.RhetosBuilderDefaultLogProvider()) // Delegate RhetosHost logging to standard NetCore targets.
                .ConfigureConfiguration(builder => builder
                    .MapNetCoreConfiguration(configuration)
                    // The "local" file is intended for developer/machine-specific database connection string, and other test settings.
                    // It should not be committed to source control (see .gitignore).
                    .AddJsonFile("rhetos-app.local.settings.json"))
                .ConfigureContainer(builder =>
                {
                    // Configuring standard Rhetos system services to work with unit tests:
                    builder.RegisterType<ProcessUserInfo>().As<Rhetos.Utilities.IUserInfo>();
                    builder.RegisterType<Rhetos.Utilities.ConsoleLogProvider>().As<ILogProvider>();
                    // Registering custom components for Bookstore application:
                    builder.RegisterType<Bookstore.SmtpMailSender>().As<Bookstore.IMailSender>(); // Application uses SMTP implementation for sending mails. The registration will be overridden in unit tests by fake component.
                    builder.Register(context => context.Resolve<Rhetos.Utilities.IConfiguration>().GetOptions<Bookstore.MailOptions>()).SingleInstance(); // Standard pattern for registering an options class.
                });

        }
    }
}
