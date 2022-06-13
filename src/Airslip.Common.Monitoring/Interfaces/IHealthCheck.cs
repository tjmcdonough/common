using Airslip.Common.Monitoring.Models;
using System.Threading.Tasks;

namespace Airslip.Common.Monitoring.Interfaces
{
    public interface IHealthCheck
    {
        Task<HealthCheckResults> Execute();
    }
}