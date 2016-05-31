using IdentityServer4.Core.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace IdSvrHost.Configuration
{
    public class Clients
    {
        private readonly AppSettings _settings;

        public Clients(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }
        public IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "AureliaAspNetApp",
                    ClientName = "Aurelia AspNet App",
                    ClientUri="http://identityserver1.io",
                    ClientSecrets = new List<Secret> {
                         new Secret(_settings.ClientSecret.Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = true,
                    AllowAccessToAllScopes=false,
                    AllowRememberConsent = true,
                    RedirectUris = new List<string> {
                        _settings.MVC,
                    },
                    PostLogoutRedirectUris = new List<string> {
                        _settings.MVC,
                    },
                      AllowedCorsOrigins = new List<string>
                    {
                        _settings.MVC,
                    },
                    AllowedScopes = new List<string> {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.Roles.Name,

                       "crm"

                    }
                },
                 //aurelia client
                new Client
                {
                    ClientId = "AureliaWebsite",
                    ClientName = "Aurelia Website",
                    ClientUri="http://identityserver2.io",
                    ClientSecrets = new List<Secret> {
                         new Secret("secret".Sha256())
                    },
                     AllowedGrantTypes = GrantTypes.Implicit,
                     AllowAccessTokensViaBrowser= true,
                    RequireConsent = true,
                    //AllowAccessToAllScopes=true,
                    AllowRememberConsent = false,
                    RedirectUris = new List<string> {
                        _settings.MVC,
                        _settings.AureliaWebSiteApp,
                        _settings.NodeJsApp,


                    },
                    PostLogoutRedirectUris = new List<string> {
                        _settings.MVC,
                        _settings.AureliaWebSiteApp,
                        _settings.NodeJsApp,

                    },
                      AllowedCorsOrigins = new List<string>
                    {
                        _settings.MVC,
                        _settings.AureliaWebSiteApp,
                        _settings.NodeJsApp,
                    },
                    AllowedScopes = new List<string> {
                       StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.Roles.Name,
                        StandardScopes.Phone.Name,

                        "crm"

                    }
                },

               
            };
        }
    }
}