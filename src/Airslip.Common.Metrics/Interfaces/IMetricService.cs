using Airslip.Common.Metrics.Implementations;

namespace Airslip.Common.Metrics.Interfaces;

public interface IMetricService
{
    MetricLogger StartActivity(string activityName);
}