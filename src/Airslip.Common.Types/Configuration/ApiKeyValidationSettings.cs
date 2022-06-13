namespace Airslip.Common.Types.Configuration
{
    public record ApiKeyValidationSettings
    {
        public string VerificationToken { get; set; } = string.Empty;
        
        public string ValidationEndpoint { get; set; } = string.Empty;
    }
}