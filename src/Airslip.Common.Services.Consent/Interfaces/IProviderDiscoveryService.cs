using Airslip.Common.Services.Consent.Configuration;
using Airslip.Common.Services.Consent.Enums;

namespace Airslip.Common.Services.Consent.Interfaces
{
    public interface IProviderDiscoveryService
    {
        ProviderDetails GetProviderDetails(Provider provider);
    }

    public record ProviderDetails(string Uri, string ApiKey, ProviderSetting ProviderSetting, string CallbackUrl);
}