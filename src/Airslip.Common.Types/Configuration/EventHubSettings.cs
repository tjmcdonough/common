namespace Airslip.Common.Types.Configuration
{
    public record EventHubSettings
    {
        public string ConnectionString { get; init; } = string.Empty;
        public string HubName { get; set; } = string.Empty;
    }
}