using Airslip.Common.ImageGeneration.Implementations;
using Airslip.Common.ImageGeneration.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.ImageGeneration.Extensions
{
    public static class HostedExtensions
    {
        /// <summary>
        /// Add authorisation for a hosted service
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddQrCode(this IServiceCollection services)
        {
            services
                .AddScoped<IQrCodeService, QrCodeService>();

            return services;
        }
    }
}