using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdSvrHost
{
    public class AppSettings
    {
        public string SiteTitle { get; set; }
        public string BaseURI { get; set; }

        //TODO would make sense to provide settings for the ports of api, mvc, ...

        public string API
        {
            get
            {
                return "http://" + BaseURI + ":44315";
            }
        }
        public string MVC
        {
            get
            {
                return "http://" + BaseURI + ":49849";
            }
        }
        public string MVCSTSCallback
        {
            get
            {
                return MVC + "/stscallback";
            }
        }
        public string AureliaWebSiteApp
        {
            get
            {
                return "http://" + BaseURI + ":9000";

            }
        }
        public string NodeJsApp
        {
            get
            {
                return "http://" + BaseURI + ":4000";

            }
        }

        public string ClientSecret
        {
            get
            {
                return "secret";

            }
        }
        public string IssuerUri
        {
            get
            {
                return "http://myapp/identity";

            }
        }

        public string STSOrigin
        {
            get
            {
                return "http://" + BaseURI + ":22530";

            }
        }
        public string STS
        {
            get
            {
                return STSOrigin;

            }
        }
        public string STSTokenEndpoint
        {
            get
            {
                return STS + "/connect/token";

            }
        }
        public string STSAuthorizationEndpoint
        {
            get
            {
                return STS + "/connect/authorize";

            }
        }

        public string STSUserInfoEndpoint
        {
            get
            {
                return STS + "/connect/userinfo";

            }
        }
        public string STSEndSessionEndpoint
        {
            get
            {
                return STS + "/connect/endsession";

            }
        }
        public string STSRevokeTokenEndpoint
        {
            get
            {
                return STS + "/connect/revocation";

            }
        }
    }
}
