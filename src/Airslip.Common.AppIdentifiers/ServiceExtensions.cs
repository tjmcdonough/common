using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.AppIdentifiers
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAppIdentifiers(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services
                .Configure<SettingCollection<AppleAppIdentifierSetting>>(configuration.GetSection("AppleAppIdentifierSettings"))
                .Configure<AndroidAppIdentifierSettings>(configuration.GetSection(nameof(AndroidAppIdentifierSettings)))
                .AddScoped<IAppleAppIdentificationService, AppleAppIdentificationService>();

            return services;
        }
    }
}