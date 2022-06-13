using Airslip.Common.Monitoring.Models;
using System.Threading.Tasks;

namespace Airslip.Common.Monitoring.Interfaces
{
    public interface IHealthCheckService
    {
        Task<HealthCheckResponse> CheckServices();
    }
}