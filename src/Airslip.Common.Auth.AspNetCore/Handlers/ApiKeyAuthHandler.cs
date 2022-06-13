using Airslip.Common.Auth.AspNetCore.Schemes;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.AspNetCore.Handlers
{
    public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
    {
        private readonly IApiKeyRequestHandler _apiKeyRequestHandler;

        public ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IApiKeyRequestHandler apiKeyRequestHandler) 
            : base(options, logger, encoder, clock)
        {
            _apiKeyRequestHandler = apiKeyRequestHandler;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationResult = await _apiKeyRequestHandler.Handle(Request);

            switch (authenticationResult.AuthResult)
            {
                case AuthResult.Success:
                    return AuthenticateResult.Success(new AuthenticationTicket(authenticationResult.Principal!,
                        ApiKeyAuthenticationSchemeOptions.ApiKeyScheme));
                case AuthResult.Fail:
                    return AuthenticateResult.Fail(authenticationResult.Message ?? "Unable to authenticate");
                default:
                    return AuthenticateResult.NoResult();
            }
        }
    }
}