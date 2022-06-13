using Airslip.Common.Monitoring.Interfaces;
using Airslip.Common.Monitoring.Models;
using Serilog;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Monitoring.Implementations
{
    public class HealthCheckService : IHealthCheckService
    {
        private readonly IEnumerable<IHealthCheck> _healthChecks;
        private readonly ILogger _logger;

        public HealthCheckService(IEnumerable<IHealthCheck> healthChecks, ILogger logger)
        {
            _healthChecks = healthChecks;
            _logger = logger;
        }
        
        public async Task<HealthCheckResponse> CheckServices()
        {
            Stopwatch sw = new();
            HealthCheckResponse result = new();
            
            foreach (IHealthCheck healthCheck in _healthChecks)
            {
                sw.Restart();
                HealthCheckResults healthCheckResult = await healthCheck.Execute();

                if (healthCheckResult.Results.Any(o => !o.Ok))
                {
                    healthCheckResult.Results
                        .Where(o => !o.Ok)
                        .ToList()
                        .ForEach(status =>
                        {
                            _logger.Warning(status.Exception,
                                "Health check failed: Name {Name}", status.Name);
                        });
                }
                
                sw.Stop();
                
                result.HealthChecks.Add(new HealthCheck(healthCheckResult, sw.ElapsedMilliseconds));   
            }

            return result;
        }
    }
}