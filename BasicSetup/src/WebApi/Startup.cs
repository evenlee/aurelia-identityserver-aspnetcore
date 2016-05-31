using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;

namespace WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                //.AddJsonFile("appsettings.json")
                .AddEnvironmentVariables("Aurelia_Sample_");

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);


            //RC2
            var scopePolicy = new AuthorizationPolicyBuilder()
                                .RequireClaim("scope", "crm")
                                .Build();

            //TODO RC2
            services.Configure<AppSettings>(options => Configuration.GetSection("AppSettings").Bind(options));


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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<AppSettings> settings)
        {
            //the baseURI setting is injected in the app settings from an environment variable
            //the reason is that at build time we don't know the ip address of the docker host
            settings.Value.BaseURI = Configuration["BaseURI"];

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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
                RequireHttpsMetadata=false,
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

        // Entry point for the application.
        //RC 2
        //public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
