using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNet.Authorization;
using WebApi.AuthorizationHandlers;

namespace WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            //services.AddCors();
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("Cors", builder =>
            //    {
            //        builder.WithOrigins(new string[] { "http://localhost:49849", "http://localhost:9999" });
            //        builder.AllowAnyHeader();
            //        builder.AllowAnyMethod();
            //    });
            //});
            services.AddMvc().AddJsonOptions(opts =>
           {
               opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
           });

            //authorization handler
            //services.AddTransient<IAuthorizationHandler, CrmAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //TODO refine cors
            app.UseCors(policy =>
            {
                policy.WithOrigins(new string[] { "http://localhost:49849", "http://localhost:9000", "http://localhost:9999" });
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });

            //app.UseCors("Cors");

            // custom middleware to checked each call as it comes in.



            app.UseIISPlatformHandler();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseIdentityServerAuthentication(options =>
            {
                options.Authority = Constants.STS;
                options.ScopeName = "crm";
                options.ScopeSecret = "secret";

                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;
            });




            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
