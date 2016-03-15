using IdentityServer4.Core.Models;
using System.Collections.Generic;

namespace IdSvrHost.Configuration
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "AureliaAspNetApp",
                    ClientName = "Aurelia AspNet App",
                    ClientUri="http://identityserver1.io",
                    ClientSecrets = new List<Secret> {
                         new Secret(AppConstants.ClientSecret.Sha256())
                    },
                    Flow = Flows.AuthorizationCode,
                    RequireConsent = true,
                    AllowAccessToAllScopes=false,
                    AllowRememberConsent = true,
                    RedirectUris = new List<string> {
                        AppConstants.MVC,
                    },
                    PostLogoutRedirectUris = new List<string> {
                        AppConstants.MVC,
                    },
                      AllowedCorsOrigins = new List<string>
                    {
                        AppConstants.MVC,
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
                    Flow = Flows.Implicit,
                    RequireConsent = true,
                    //AllowAccessToAllScopes=true,
                    AllowRememberConsent = false,
                    RedirectUris = new List<string> {
                        AppConstants.MVC,
                        AppConstants.AureliaWebSiteApp,
                        AppConstants.NodeJsApp,


                    },
                    PostLogoutRedirectUris = new List<string> {
                        AppConstants.MVC,
                        AppConstants.AureliaWebSiteApp,
                        AppConstants.NodeJsApp,

                    },
                      AllowedCorsOrigins = new List<string>
                    {
                        AppConstants.MVC,
                        AppConstants.AureliaWebSiteApp,
                        AppConstants.NodeJsApp,
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