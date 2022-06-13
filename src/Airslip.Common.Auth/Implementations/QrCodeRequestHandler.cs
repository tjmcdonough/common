using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Exceptions;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Implementations
{
    public class QrCodeRequestHandler : IQrCodeRequestHandler
    {
        private readonly ITokenValidator<QrCodeToken> _tokenValidator;

        public QrCodeRequestHandler(ITokenValidator<QrCodeToken> tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }
        
        public async Task<KeyAuthenticationResult> Handle(HttpRequest request)
        {
            // Get the raw querystring
            string qs = request.QueryString.ToString()[1..];

            if (qs.IsNullOrWhitespace())
                return KeyAuthenticationResult.NoResult();
            
            try
            {
                ClaimsPrincipal? apiKeyPrincipal = await _tokenValidator.GetClaimsPrincipalFromToken(qs, 
                    AirslipSchemeOptions.QrCodeAuthScheme, 
                    AirslipSchemeOptions.ThisEnvironment);

                return apiKeyPrincipal == null
                    ? KeyAuthenticationResult.Fail("QR Code invalid")
                    : KeyAuthenticationResult.Valid(apiKeyPrincipal);
            }
            catch (ArgumentException)
            {
                return KeyAuthenticationResult.Fail("QR Code invalid");
            }
            catch (EnvironmentUnsupportedException)
            {
                return KeyAuthenticationResult.Fail("Environment Unsupported");
            }
        }
    }
}