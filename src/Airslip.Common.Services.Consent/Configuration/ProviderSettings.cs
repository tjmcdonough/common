namespace Airslip.Common.Services.Consent.Configuration
{
    public record ProviderSetting
    {
        public string PublicApiSettingName { get; set; } = string.Empty;
        public string ConsentRouteFormat { get; set; } = string.Empty;
        public string AuthoriseRouteFormat { get; set; } = string.Empty;
    }
}        
    