using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Consent.Interfaces
{
    public interface IProviderConsentService
    {
        Task<IResponse?> GetConsentUrl(Provider provider, string bankId, string? countryCode, CancellationToken cancellationToken);
        Task<IResponse?> GetConsentUrl(Provider provider, string bankId, string? countryCode, string? callbackUrl, CancellationToken cancellationToken);
        Task<IResponse?> ApproveConsent(Provider provider, string consent, string bankId, string userId,
            string providerUserId, CancellationToken cancellationToken);
    }
}