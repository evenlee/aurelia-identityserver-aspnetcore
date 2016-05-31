﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using IdSvrHost.Configuration;
using IdSvrHost.Extensions;

namespace IdSvrHost
{
    public class Program
    {
        private static IConfigurationRoot Configuration;
        public static void Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //    .AddCommandLine(args)
            //    .Build();
            ;
            var builder = new ConfigurationBuilder()
               //.AddJsonFile("appsettings.json")
               //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables("Aurelia_Sample_")
               .AddEnvironmentVariables("ASPNETCORE_");


            //TODO check how to use environment vars here
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();

            Console.WriteLine("STS_URL " + Configuration["STS_URL"]);

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(Configuration)
                .UseUrls(Configuration["STS_URL"])
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                //.UseStartup<Startup>()
                .Configure(configureApplication)
                .ConfigureServices(configureServices)
                .ConfigureLogging(configureLogging)

                .Build();

            host.Run();
        }

        private static void configureLogging(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Trace);
            loggerFactory.AddDebug(LogLevel.Trace);
        }

        private static void configureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<AppSettings>(c => c.BaseURI = Configuration["BaseURI"]);

            var cert = new X509Certificate2(
                Path.Combine(Directory.GetCurrentDirectory(), "idsrv4test.pfx"), "idsrv3test");

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var builder = services.AddIdentityServer(options =>
            {
                options.SigningCertificate = cert;
                options.RequireSsl = false;
                options.SiteName = "IDS";
                options.Endpoints.EnableIdentityTokenValidationEndpoint = true;


                options.EventsOptions = new IdentityServer4.Core.Configuration.EventsOptions
                {
                    RaiseSuccessEvents = true,
                    RaiseErrorEvents = true,
                    RaiseFailureEvents = true,
                    RaiseInformationEvents = true
                };


            });
            //feels a bit artificial: 
            // we need access to the AppSettings provider inside the service configuration...

            var sp = services.BuildServiceProvider();

            IOptions<AppSettings> settings = sp.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            System.Console.WriteLine("base uri " + settings.Value.BaseURI);

            builder.AddInMemoryClients(new Clients((settings)).Get());
            builder.AddInMemoryScopes(Scopes.Get());

            builder.AddInMemoryUsers(Users.Get());
            builder.AddCustomGrantValidator<CustomGrantValidator>();

            // for the UI
            services
                .AddMvc()
                .AddRazorOptions(razor =>
                {
                    razor.ViewLocationExpanders.Add(new IdSvrHost.UI.CustomViewLocationExpander());
                });
            services.AddTransient<IdSvrHost.UI.Login.LoginService>();
        }

        private static void configureApplication(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            //TODO refine CORS
            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });
            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}