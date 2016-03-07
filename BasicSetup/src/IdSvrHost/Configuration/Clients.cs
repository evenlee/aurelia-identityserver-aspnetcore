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
                         new Secret(IdSvrConstants.Constants.ClientSecret.Sha256())
                    },
                    Flow = Flows.AuthorizationCode,
                    RequireConsent = true,
                    AllowAccessToAllScopes=false,
                    AllowRememberConsent = true,
                    RedirectUris = new List<string> {
                        IdSvrConstants.Constants.MVC,
                    },
                    PostLogoutRedirectUris = new List<string> {
                        IdSvrConstants.Constants.MVC,
                    },
                      AllowedCorsOrigins = new List<string>
                    {
                        IdSvrConstants.Constants.MVC,
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
                        IdSvrConstants.Constants.MVC,
                        IdSvrConstants.Constants.AureliaWebSiteApp,
                        IdSvrConstants.Constants.NodeJsApp,


                    },
                    PostLogoutRedirectUris = new List<string> {
                        IdSvrConstants.Constants.MVC,
                        IdSvrConstants.Constants.AureliaWebSiteApp,
                        IdSvrConstants.Constants.NodeJsApp,

                    },
                      AllowedCorsOrigins = new List<string>
                    {
                        IdSvrConstants.Constants.MVC,
                        IdSvrConstants.Constants.AureliaWebSiteApp,
                        IdSvrConstants.Constants.NodeJsApp,
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