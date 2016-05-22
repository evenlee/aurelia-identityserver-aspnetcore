using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdSvrHost.Configuration;
using IdSvrHost.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using IdentityServer4.Core.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace IdSvrHost
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;
        public IConfiguration Configuration { get; set; }

        public Startup( IHostingEnvironment env)
        {
            _environment = env;

            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddEnvironmentVariables("Aurelia_Sample_")
                 .AddEnvironmentVariables("ASPNETCORE_");
            Configuration = builder.Build();

        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.Configure<AppSettings>(options => Configuration.GetSection("AppSettings").Bind(options));
            services.Configure<AppSettings>((a) =>
                 {
                     a.BaseURI = Configuration["BaseURI"];

                 });
            System.Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxx " + Path.Combine(_environment.ContentRootPath, "idsrv4test.pfx"));

            var cert = new X509Certificate2(Path.Combine(_environment.ContentRootPath, "idsrv4test.pfx"), "idsrv3test");
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
            //feels a bit artificial: we need access to the AppSettings provider inside the service configuration...

            var sp = services.BuildServiceProvider();
          
            IOptions<AppSettings> settings = sp.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            settings.Value.BaseURI = Configuration["BaseURI"];

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

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            loggerFactory.AddConsole(LogLevel.Trace);
            loggerFactory.AddDebug(LogLevel.Trace);
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

        private void ConfigureAdditionalIdProviders(IApplicationBuilder app)
        {
            //var fbAuthOptions = new FacebookAuthenticationOptions
        }

      
    }
}
