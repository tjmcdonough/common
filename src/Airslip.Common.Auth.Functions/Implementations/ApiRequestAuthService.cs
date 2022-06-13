using Airslip.Common.Auth.Functions.Data;
using Airslip.Common.Auth.Functions.Extensions;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.Azure.Functions.Worker.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Functions.Implementations
{
    public class ApiRequestAuthService : IApiRequestAuthService
    {
        private readonly IApiKeyRequestDataHandler _requestDataHandler;
        private readonly ITokenDecodeService<ApiKeyToken> _decodeService;
        private readonly IApiAccessRights _apiAccessRights;

        public ApiRequestAuthService(IApiKeyRequestDataHandler requestDataHandler,
            ITokenDecodeService<ApiKeyToken> decodeService,
            IApiAccessRights apiAccessRights)
        {
            _requestDataHandler = requestDataHandler;
            _decodeService = decodeService;
            _apiAccessRights = apiAccessRights;
        }

        public Task<KeyAuthenticationResult> Handle(HttpRequestData requestData)
        {
            return Handle(string.Empty, requestData);
        }

        public async Task<KeyAuthenticationResult> Handle(string functionNamed, HttpRequestData requestData)
        {
            KeyAuthenticationResult authenticationResult = await _requestDataHandler.Handle(requestData);
            
            if (authenticationResult.AuthResult != AuthResult.Success)
            {
                return authenticationResult;
            }

            // Get the token
            ApiKeyToken token = _decodeService.GetCurrentToken();
            
            // Validate against what is allowed in
            bool succeeded = _apiAccessRights
                .AccessDefinitions
                .Where(o => o.Named is null || o.Named.Equals(functionNamed))
                .Any(o => o.ValidateAccess(token));
            
            return succeeded ? authenticationResult : KeyAuthenticationResult.Fail("Api Key authentication failed");
        }
    }
}