namespace Airslip.Common.Repository.Configuration
{
    public record RepositorySettings
    {
        public bool IncludeMetrics { get; set; } = false;
    }
}