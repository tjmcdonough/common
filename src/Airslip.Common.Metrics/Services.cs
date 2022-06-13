using Airslip.Common.Metrics.Configuration;
using Airslip.Common.Metrics.Implementations;
using Airslip.Common.Metrics.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Metrics
{
    public static class Services
    {
        public static IServiceCollection AddMetrics(this IServiceCollection serviceCollection,  
            IConfiguration configuration)
        {
            return serviceCollection
                .AddScoped<IMetricService, MetricService>()
                .Configure<MetricSettings>(configuration.GetSection(nameof(MetricSettings)));
        }
    }
}