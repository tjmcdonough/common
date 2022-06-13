using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Exceptions;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Airslip.Common.Auth.Functions.Implementations
{
    public class ApiKeyRequestDataHandler : IApiKeyRequestDataHandler
    {
        private readonly ITokenValidator<ApiKeyToken> _tokenValidator;
        private readonly IFunctionContextAccessor _functionContextService;

        public ApiKeyRequestDataHandler(ITokenValidator<ApiKeyToken> tokenValidator, 
            IFunctionContextAccessor functionContextService)
        {
            _tokenValidator = tokenValidator;
            _functionContextService = functionContextService;
        }
        
        public async Task<KeyAuthenticationResult> Handle(HttpRequestData request)
        {
            _functionContextService.Headers = request.Headers;
            _functionContextService.QueryString = 
                HttpUtility.ParseQueryString(request.Url.OriginalString);
            
            if (!request.Headers.Contains(AirslipSchemeOptions.ApiKeyHeaderField))
            {
                return KeyAuthenticationResult.Fail($"{AirslipSchemeOptions.ApiKeyHeaderField} header not found");
            }

            List<string> headerValue = request
                .Headers.GetValues(AirslipSchemeOptions.ApiKeyHeaderField)
                .ToList();

            try
            {
                ClaimsPrincipal? apiKeyPrincipal = await _tokenValidator
                    .GetClaimsPrincipalFromToken(headerValue.First(), 
                        AirslipSchemeOptions.ApiKeyScheme, 
                        AirslipSchemeOptions.ThisEnvironment);
                
                _functionContextService.User = apiKeyPrincipal;
                
                return apiKeyPrincipal == null ? 
                    KeyAuthenticationResult.Fail("Api key invalid") : 
                    KeyAuthenticationResult.Valid(apiKeyPrincipal);
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