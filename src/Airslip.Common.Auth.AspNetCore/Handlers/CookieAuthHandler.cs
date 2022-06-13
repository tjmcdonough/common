using Airslip.Common.Auth.AspNetCore.Interfaces;
using Airslip.Common.Auth.AspNetCore.Schemes;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.AspNetCore.Handlers
{
    public class CookieAuthHandler : AuthenticationHandler<CookieAuthenticationSchemeOptions>
    {
        private readonly ICookieRequestHandler _cookieRequestHandler;

        public CookieAuthHandler(IOptionsMonitor<CookieAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ICookieRequestHandler cookieRequestHandler) 
            : base(options, logger, encoder, clock)
        {
            _cookieRequestHandler = cookieRequestHandler;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationResult = await _cookieRequestHandler.Handle(Request);

            switch (authenticationResult.AuthResult)
            {
                case AuthResult.Success:
                    return AuthenticateResult.Success(new AuthenticationTicket(authenticationResult.Principal!,
                        CookieAuthenticationSchemeOptions.CookieAuthScheme));
                case AuthResult.Fail:
                    return AuthenticateResult.Fail(authenticationResult.Message ?? "Unable to authenticate");
                default:
                    return AuthenticateResult.NoResult();
            }
        }
    }
}