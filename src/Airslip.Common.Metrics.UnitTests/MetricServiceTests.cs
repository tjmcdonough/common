using Airslip.Common.Metrics.Configuration;
using Airslip.Common.Metrics.Implementations;
using Airslip.Common.Metrics.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using Xunit;

namespace Airslip.Common.Metrics.UnitTests;

public class MetricServiceTests
{
    [Fact]
    public void Does_log_completion_with_metric_result()
    {
        Mock<ILogger> logger = new();
        
        IOptions<MetricSettings> someOptions = Options.Create(new MetricSettings()
        {
            IncludeMetrics = true
        });
        IMetricService metricService = new MetricService(logger.Object, someOptions);

        using MetricLogger metricLogger = metricService.StartActivity("Some activity");
        metricLogger.LogMetric("Some metric");
        
        metricLogger.CompleteMetric();

        logger.Invocations.Count.Should().Be(3);
    }
    
    [Fact]
    public void Does_log_completion_with_metric_result_using_closure()
    {
        Mock<ILogger> logger = new();
        
        IOptions<MetricSettings> someOptions = Options.Create(new MetricSettings()
        {
            IncludeMetrics = true
        });
        IMetricService metricService = new MetricService(logger.Object, someOptions);

        using (MetricLogger metricLogger = metricService.StartActivity("Some activity"))
        {
            metricLogger.LogMetric("Some metric");
        }

        logger.Invocations.Count.Should().Be(3);
    }
}