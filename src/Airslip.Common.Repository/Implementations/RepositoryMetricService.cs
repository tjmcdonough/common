using Airslip.Common.Repository.Configuration;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;
using System.Diagnostics;

namespace Airslip.Common.Repository.Implementations;

public class RepositoryMetricService : IRepositoryMetricService
{
    private readonly ILogger _logger;
    private readonly Stopwatch _stopwatch;
    private readonly RepositorySettings _settings;

    public RepositoryMetricService(ILogger logger, IOptions<RepositorySettings> options)
    {
        _logger = logger;
        _stopwatch = new Stopwatch();
        _settings = options.Value ?? new RepositorySettings();
    }
    
    public void LogMetric(string activityName, string metricName, RepositoryMetricType metricType)
    {
        if (!_settings.IncludeMetrics) return;
        _logger.Debug("{MetricTime}ms: {ActivityName} - {MetricName} - {MetricType}", _stopwatch.ElapsedMilliseconds, 
            activityName, metricName, metricType.ToString());
    }

    public void StartClock()
    {
        if (!_settings.IncludeMetrics) return;
        _stopwatch.Restart();
    }

    public void StopClock()
    {
        if (!_settings.IncludeMetrics) return;
        _stopwatch.Stop();
    }
}