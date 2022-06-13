using Airslip.Common.Repository.Types.Enums;

namespace Airslip.Common.Repository.Types.Interfaces;

public interface IRepositoryMetricService
{
    void LogMetric(string activityName, string metricName, RepositoryMetricType metricType);
    void StartClock();
    void StopClock();
}