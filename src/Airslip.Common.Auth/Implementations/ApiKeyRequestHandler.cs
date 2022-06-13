using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Exceptions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Implementations
{
    public class ApiKeyRequestHandler : IApiKeyRequestHandler
    {
        private readonly ITokenValidator<ApiKeyToken> _tokenValidator;

        public ApiKeyRequestHandler(ITokenValidator<ApiKeyToken> tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }
        
        public async Task<KeyAuthenticationResult> Handle(HttpRequest request)
        {
            if (!request.Headers.ContainsKey(AirslipSchemeOptions.ApiKeyHeaderField))
            {
                return KeyAuthenticationResult
                    .NoResult();
            }

            KeyValuePair<string, StringValues> apiKeyToken = request
                .Headers
                .First(o => o.Key == AirslipSchemeOptions.ApiKeyHeaderField);

            try
            {
                ClaimsPrincipal? apiKeyPrincipal = await _tokenValidator
                    .GetClaimsPrincipalFromToken(apiKeyToken.Value.First(), 
                        AirslipSchemeOptions.ApiKeyScheme, 
                        AirslipSchemeOptions.ThisEnvironment);

                return apiKeyPrincipal == null
                    ? KeyAuthenticationResult.Fail("Api key invalid")
                    : KeyAuthenticationResult.Valid(apiKeyPrincipal);
                
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