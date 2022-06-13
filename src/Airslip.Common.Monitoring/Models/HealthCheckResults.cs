using System.Collections.Generic;

namespace Airslip.Common.Monitoring.Models
{
    public record HealthCheckResults(IEnumerable<HealthCheckResult> Results);
}