using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Services.Consent.Extensions;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Services.Consent.Models;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using Airslip.Common.Utilities.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Consent.Implementations
{
    public class ProviderConsentService : IProviderConsentService
    {
        private readonly IProviderDiscoveryService _discoveryService;
        private readonly HttpClient _httpClient;
        private readonly UserToken _userToken;

        public ProviderConsentService(IHttpClientFactory factory,
            ITokenDecodeService<UserToken> tokenDecodeService,
            IProviderDiscoveryService discoveryService)
        {
            _httpClient = factory.CreateClient(nameof(ProviderConsentService));
            _discoveryService = discoveryService;
            _userToken = tokenDecodeService.GetCurrentToken();
        }
        
        public Task<IResponse?> GetConsentUrl(Provider provider, string bankId, 
            string? countryCode, CancellationToken cancellationToken)
        {
            return GetConsentUrl(provider, bankId, countryCode, null, cancellationToken);
        }

        public async Task<IResponse?> GetConsentUrl(Provider provider, string bankId, string? countryCode, string? callbackUrl,
            CancellationToken cancellationToken)
        {
            ProviderDetails providerDetails = _discoveryService.GetProviderDetails(provider);
            
            string url = $"{providerDetails.Uri}/{providerDetails.ProviderSetting.ConsentRouteFormat}";
            
            // Apply some dynamic replacement
            url = url.ApplyReplacements(new Dictionary<string, string>
            {
                {"entityId", _userToken.EntityId},
                {"airslipUserType", _userToken.AirslipUserType.ToString().ToLower()},
                {"userId", _userToken.UserId},
                {"bankId", bankId},
                {"callbackUrl", callbackUrl ?? providerDetails.CallbackUrl}
            });
            
            HttpActionResult apiCallResponse = await _httpClient
                .GetApiRequest<AccountAuthorisationResponse>(url, providerDetails.ApiKey, cancellationToken);
            
            return apiCallResponse.Response;
        }

        public async Task<IResponse?> ApproveConsent(Provider provider, string consent, 
            string bankId, string userId, string providerUserId, CancellationToken cancellationToken)
        {
            ProviderDetails providerDetails = _discoveryService.GetProviderDetails(provider);

            string url = $"{providerDetails.Uri}/{providerDetails.ProviderSetting.AuthoriseRouteFormat}";
                
            // Apply some dynamic replacement
            url = url.ApplyReplacements(new Dictionary<string, string>
            {
                {"consent", consent},
                {"userId", userId},
                {"providerUserId", providerUserId},
                {"bankId", bankId}
            });
            
            HttpActionResult apiCallResponse = await _httpClient
                .GetApiRequest<AccountAuthorisedResponse>(url, providerDetails.ApiKey, cancellationToken);
            
            return apiCallResponse.Response;
        }
    }
}