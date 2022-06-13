namespace Airslip.Common.Metrics.Configuration;

public record MetricSettings
{
    public bool IncludeMetrics { get; set; } = false;
    public long Threshold { get; set; } = 1000;
}