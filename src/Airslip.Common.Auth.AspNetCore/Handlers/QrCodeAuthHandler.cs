using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using QrCodeAuthenticationSchemeOptions = Airslip.Common.Auth.AspNetCore.Schemes.QrCodeAuthenticationSchemeOptions;

namespace Airslip.Common.Auth.AspNetCore.Handlers
{
    public class QrCodeAuthHandler : AuthenticationHandler<QrCodeAuthenticationSchemeOptions>
    {
        private readonly IQrCodeRequestHandler _qrCodeRequestHandler;

        public QrCodeAuthHandler(IOptionsMonitor<QrCodeAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, 
            IQrCodeRequestHandler qrCodeRequestHandler) 
            : base(options, logger, encoder, clock)
        {
            _qrCodeRequestHandler = qrCodeRequestHandler;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationResult = await _qrCodeRequestHandler.Handle(Request);

            switch (authenticationResult.AuthResult)
            {
                case AuthResult.Success:
                    return AuthenticateResult.Success(new AuthenticationTicket(authenticationResult.Principal!,
                        QrCodeAuthenticationSchemeOptions.QrCodeAuthScheme));
                case AuthResult.Fail:
                    return AuthenticateResult.Fail(authenticationResult.Message ?? "Unable to authenticate");
                default:
                    return AuthenticateResult.NoResult();
            }
        }
    }
}