using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace WebApi
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

            Console.WriteLine("API_URL " + Configuration["API_URL"]);

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls(Configuration["API_URL"])
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
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.Configure<AppSettings>(c => c.BaseURI = Configuration["BaseURI"]);


            //RC2
            var scopePolicy = new AuthorizationPolicyBuilder()
                                .RequireClaim("scope", "crm")
                                .Build();
            
            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
        }

        private static void configureApplication(IApplicationBuilder app)
        {

            IOptions<AppSettings> settings = app.ApplicationServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            System.Console.WriteLine("web api base uri " + settings.Value.BaseURI);

            //TODO refine cors
            app.UseCors(policy =>
            {
                policy.WithOrigins(new string[] { settings.Value.MVC, settings.Value.AureliaWebSiteApp });
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });

            //app.UseCors("Cors");

            // custom middleware to checked each call as it comes in.


            //TODO RC2
            //app.UseIISPlatformHandler();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Authority = settings.Value.STS,
                RequireHttpsMetadata = false,
                Audience = settings.Value.STS + "/resources",
                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                },
                //RC2
                //ScopeName = "crm",
                //ScopeSecret = "secret",

                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });




            app.UseMvc();
        }
    }
}
