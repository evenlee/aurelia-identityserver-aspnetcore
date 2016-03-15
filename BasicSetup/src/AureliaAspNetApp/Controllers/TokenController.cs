using IdentityModel.Client;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AureliaAspNetApp.Controllers
{
    public class TokenController: Controller
    {
        public class TokenExchangeInput
        {
            public string Code { get; set; }
            public string RedirectUri { get; set; }
            public string ClientId { get; set; }
        }

        public class TokenResponse
        {
            public string Token { get; set; }
        }

        [HttpPost()]
        public async Task<ActionResult> Exchange([FromBody]TokenExchangeInput tokenExchangeInput)
        {
            var client = new TokenClient(Constants.STSTokenEndpoint, "AureliaAspNetApp", Constants.ClientSecret);
            var tokenResponse = await client.RequestAuthorizationCodeAsync(tokenExchangeInput.Code, tokenExchangeInput.RedirectUri);
            return Json(new TokenResponse { Token = tokenResponse.AccessToken });
        }
    }
}
