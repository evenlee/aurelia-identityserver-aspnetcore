using IdentityServer4.Core.Models;
using System.Collections.Generic;

namespace IdSvrHost.Configuration

{
    public class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            var scopes=  new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.ProfileAlwaysInclude,
                StandardScopes.EmailAlwaysInclude,
                StandardScopes.OfflineAccess,
                StandardScopes.RolesAlwaysInclude,

                new Scope
                {
                    Name = "crm",
                    DisplayName = "Crm",
                    Description = "Access to CRM data",
                    Type = ScopeType.Resource,

                    ScopeSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                }
                
            };
            scopes.AddRange(StandardScopes.All);

            return scopes;
        }
    }
}