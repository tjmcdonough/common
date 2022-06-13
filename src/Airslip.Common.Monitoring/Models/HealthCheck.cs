namespace Airslip.Common.Monitoring.Models
{
    public record HealthCheck(HealthCheckResults status, long duration);
}