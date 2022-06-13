namespace Airslip.Common.Services.CosmosDb.Configuration
{
    public class CosmosDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public bool LogMetrics { get; set; }
    }
}