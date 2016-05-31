using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Options;

namespace AureliaAspNetApp
{
    public class Program
    {
        private static IConfigurationRoot Configuration;

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
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

            Console.WriteLine("MVC_URL " + Configuration["MVC_URL"]);

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls(Configuration["MVC_URL"])
                .Configure(configureApplication)
                .ConfigureServices(configureServices)
                .ConfigureLogging(configureLogging)
                .Build();

            host.Run();
        }

        private static void configureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.Configure<AppSettings>(c => c.BaseURI = Configuration["BaseURI"]);


            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

        }

        private static void configureApplication(IApplicationBuilder app)
        {


            IOptions<AppSettings> settings = app.ApplicationServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            System.Console.WriteLine("mvc base uri " + settings.Value.BaseURI);
            System.Console.WriteLine("mvc  " + settings.Value.MVC);



            app.UseApplicationInsightsRequestTelemetry();

            //the baseURI setting is injected in the app settings from an environment variable
            //the reason is that at build time we don't know the ip address of the docker host
            
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

            }
            //RC2
            //app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715


            //TODO refine cors
            app.UseCors(policy =>
            {
                policy.WithOrigins(new string[] { settings.Value.MVC });
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void configureLogging(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Trace);
            loggerFactory.AddDebug(LogLevel.Trace);
        }

    }
}
