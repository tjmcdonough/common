using Airslip.Common.Auth.AspNetCore.Interfaces;
using Airslip.Common.Auth.AspNetCore.Schemes;
using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Exceptions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.AspNetCore.Implementations
{
    public class CookieRequestHandler : ICookieRequestHandler
    {
        private readonly ITokenValidator<UserToken> _tokenValidator;
        private readonly ICookieService _cookieService;
        private readonly ILogger _logger;

        public CookieRequestHandler(ITokenValidator<UserToken> tokenValidator, ICookieService cookieService,
            ILogger logger)
        {
            _tokenValidator = tokenValidator;
            _cookieService = cookieService;
            _logger = logger;
        }
        
        public async Task<KeyAuthenticationResult> Handle(HttpRequest request)
        {
            // Find and validate the cookie value
            if (!request.Cookies.ContainsKey(CookieSchemeOptions.CookieTokenField))
            {
                return KeyAuthenticationResult
                    .NoResult();
            }

            // Read and decode the Cookie
            string tokenValue;

            try
            {
                tokenValue = _cookieService.GetCookieValue(request);
            }
            catch (Exception)
            {
                return KeyAuthenticationResult.Fail("Cookie invalid");
            }

            try
            {
                ClaimsPrincipal? cookiePrincipal = await _tokenValidator
                    .GetClaimsPrincipalFromToken(tokenValue, 
                        CookieSchemeOptions.CookieAuthScheme, 
                        AirslipSchemeOptions.ThisEnvironment);

                return cookiePrincipal == null
                    ? KeyAuthenticationResult.Fail("Cookie invalid")
                    : KeyAuthenticationResult.Valid(cookiePrincipal);
                
            }
            catch (ArgumentException)
            {
                return KeyAuthenticationResult.Fail("Api key invalid");
            }
            catch (EnvironmentUnsupportedException)
            {
                return KeyAuthenticationResult.Fail("Environment Unsupported");
            }
        }
    }
}