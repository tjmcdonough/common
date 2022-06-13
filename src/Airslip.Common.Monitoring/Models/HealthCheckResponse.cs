using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Monitoring.Models
{
    public class HealthCheckResponse
    {
        public bool AllOk =>  HealthChecks.All(o => o.status.Results.All(p => p.Ok));
        
        public long CheckDuration =>  HealthChecks.Sum(o => o.duration);

        public List<HealthCheck> HealthChecks { get; } = new List<HealthCheck>();
    }
}