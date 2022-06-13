using Airslip.Common.Services.Consent.Configuration;
using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Utilities.Extensions;
using Microsoft.Extensions.Options;

namespace Airslip.Common.Services.Consent.Implementations
{
    public class ProviderDiscoveryService : IProviderDiscoveryService
    {
        private readonly PublicApiSettings _settings;
        private readonly SettingCollection<ProviderSetting> _providerSettings;

        public ProviderDiscoveryService(IOptions<SettingCollection<ProviderSetting>> providerSettings, IOptions<PublicApiSettings> options)
        {
            _settings = options.Value;
            _providerSettings = providerSettings.Value;
        }
        
        public ProviderDetails GetProviderDetails(Provider provider)
        {
            ProviderSetting providerSetting = _providerSettings
                .GetSettingByName(provider.ToString());
            PublicApiSetting setting = _settings
                .GetSettingByName(providerSetting.PublicApiSettingName);

            string callbackUrl =
                $"{_settings.Base.ToBaseUri()}/consents/{provider.ToString().ToLower()}/authorised-institution";
            
            return new ProviderDetails(setting.ToBaseUri(), 
                setting.ApiKey, providerSetting, callbackUrl);
        }
    }
}