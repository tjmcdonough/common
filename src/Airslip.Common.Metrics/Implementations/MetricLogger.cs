using Airslip.Common.Metrics.Configuration;
using Airslip.Common.Metrics.Enums;
using Serilog;
using System;
using System.Diagnostics;

namespace Airslip.Common.Metrics.Implementations;

public class MetricLogger : IDisposable
{
    private readonly ILogger _logger;
    private readonly string _activityName;
    private readonly MetricSettings _settings;
    private readonly Stopwatch _stopwatch;

    public MetricLogger(ILogger logger, string activityName, MetricSettings settings)
    {
        _logger = logger;
        _activityName = activityName;
        _settings = settings;
        _stopwatch = new Stopwatch();
        _stopwatch.Start();

        _logDebug(_stopwatch.ElapsedMilliseconds, MetricType.Start);
    }

    public void LogMetric(string metricName)
    {
        _logDebug(_stopwatch.ElapsedMilliseconds, metricName, MetricType.InProgress);
    }

    public void CompleteMetric()
    {
        if (!_stopwatch.IsRunning) return;
        _stopwatch.Stop();
        _logDebug(_stopwatch.ElapsedMilliseconds, MetricType.Stop);

        if (_stopwatch.ElapsedMilliseconds > _settings.Threshold)
            _logError(_stopwatch.ElapsedMilliseconds, _settings.Threshold);
    }

    private void _logDebug(long metricTime, MetricType metricType)
    {
        if (!_settings.IncludeMetrics) return;
        _logger.Debug("{MetricTime}ms: {ActivityName} - {MetricType}", 
            metricTime, 
            _activityName, metricType.ToString());
    }
    
    private void _logDebug(long metricTime, string metricName, MetricType metricType)
    {
        if (!_settings.IncludeMetrics) return;
        _logger.Debug("{MetricTime}ms: {ActivityName} - {MetricName} - {MetricType}", 
            metricTime, 
            _activityName, 
            metricName, 
            metricType.ToString());
    }

    private void _logError(long metricTime, long threshold)
    {
        _logger.Error(
            "Execution time of {MetricTime}ms beyond threshold " +
            "of {Threshold}ms executing activity {ActivityName}",
            metricTime, 
            threshold, 
            _activityName);
    }
    
    public void Dispose()
    {
        CompleteMetric();
    }
}