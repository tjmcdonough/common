using Airslip.Common.Functions.Implementations;
using Airslip.Common.Functions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Functions.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Add tools for use in Function Apps
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddAirslipFunctionTools(this IServiceCollection services)
        {
            return services
                .AddScoped<IFunctionApiTools, FunctionApiTools>();
        }
    }
}