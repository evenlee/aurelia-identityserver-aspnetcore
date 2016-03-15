using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class Constants
    {

        public const string API = "https://localhost:44315";
        public const string MVC = "http://localhost:49849";
        public const string MVCSTSCallback = MVC + "/stscallback"; //not used currently
        public const string AureliaWebSiteApp = "http://localhost:9000";
        public const string NodeJsApp = "http://localhost:4000"; //for aurelia-auth-sample

        public const string ClientSecret = "secret";

        public const string IssuerUri = "http://myapp/identity";
        public const string STSOrigin = "http://localhost:22530";

        public const string STS = STSOrigin; //sometimes following is used: STSOrigin + "/identity";
        public const string STSTokenEndpoint = STS + "/connect/token";
        public const string STSAuthorizationEndpoint = STS + "/connect/authorize";
        public const string STSUserInfoEndpoint = STS + "/connect/userinfo";
        public const string STSEndSessionEndpoint = STS + "/connect/endsession";
        public const string STSRevokeTokenEndpoint = STS + "/connect/revocation";


    }
}
