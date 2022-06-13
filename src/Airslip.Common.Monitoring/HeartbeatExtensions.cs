using Airslip.Common.Monitoring.Implementations;
using Airslip.Common.Monitoring.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Monitoring
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection UseMonitoring(this IServiceCollection services)
        {
            services
                .AddApplicationInsightsTelemetry()
                .AddSingleton<IHealthCheckService, HealthCheckService>();
            
            return services;
        }
        
        public static IServiceCollection AddHealthCheck<TCheck>(this IServiceCollection services) 
            where TCheck: class, IHealthCheck
        {
            services
                .AddSingleton<IHealthCheck, TCheck>();
            
            return services;
        }
    }
}