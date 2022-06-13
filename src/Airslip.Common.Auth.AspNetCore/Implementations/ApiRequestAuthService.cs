using Airslip.Common.Auth.AspNetCore.Configuration;
using Airslip.Common.Auth.AspNetCore.Interfaces;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.AspNetCore.Implementations
{
    public class ApiRequestAuthService : IApiRequestAuthService
    {
        private readonly IApiKeyRequestHandler _requestDataHandler;
        private readonly ITokenDecodeService<ApiKeyToken> _decodeService;
        private readonly ApiAccessSettings _settings;

        public ApiRequestAuthService(IApiKeyRequestHandler requestDataHandler,
            ITokenDecodeService<ApiKeyToken> decodeService,
            IOptions<ApiAccessSettings> options)
        {
            _requestDataHandler = requestDataHandler;
            _decodeService = decodeService;
            _settings = options.Value;
        }

        public async Task<KeyAuthenticationResult> Handle(HttpRequest requestData)
        {
            var authenticationResult = await _requestDataHandler.Handle(requestData);
            
            if (authenticationResult.AuthResult != AuthResult.Success)
            {
                return authenticationResult;
            }

            // Get the token
            var token = _decodeService.GetCurrentToken();
            
            // Validate against what is allowed in
            if (_settings.AllowedTypes.Count > 0 && !_settings
                .AllowedTypes.Contains(token.AirslipUserType))
            {
                return KeyAuthenticationResult.Fail("Invalid User Type supplied");
            }
            
            if (_settings.AllowedEntities.Count > 0 && !_settings
                .AllowedEntities.Contains(token.EntityId))
            {
                return KeyAuthenticationResult.Fail("Invalid Entity supplied");
            }

            return authenticationResult;
        }
    }
}