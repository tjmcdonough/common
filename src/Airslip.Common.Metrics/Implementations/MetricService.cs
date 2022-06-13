using Airslip.Common.Metrics.Configuration;
using Airslip.Common.Metrics.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;

namespace Airslip.Common.Metrics.Implementations;

public class MetricService : IMetricService
{
    private readonly ILogger _logger;
    private readonly MetricSettings _settings;

    public MetricService(ILogger logger, IOptions<MetricSettings> options)
    {
        _logger = logger;
        _settings = options.Value ?? new MetricSettings();
    }

    public MetricLogger StartActivity(string activityName)
    {
        return new MetricLogger(_logger, activityName, _settings);
    }
}