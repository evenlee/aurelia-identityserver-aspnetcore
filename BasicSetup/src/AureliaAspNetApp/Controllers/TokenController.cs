using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AureliaAspNetApp.Controllers
{
    public class TokenController: Controller
    {
        private readonly AppSettings _settings;

        public TokenController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }
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
            var client = new TokenClient(_settings.STSTokenEndpoint, "AureliaAspNetApp", _settings.ClientSecret);
            var tokenResponse = await client.RequestAuthorizationCodeAsync(tokenExchangeInput.Code, tokenExchangeInput.RedirectUri);
            return Json(new TokenResponse { Token = tokenResponse.AccessToken });
        }
    }
}
