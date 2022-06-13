using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Hosted.Implementations;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Auth.Hosted.Extensions
{
    public static class HostedExtensions
    {
        /// <summary>
        /// Add authorisation for a hosted service
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The primary configuration where relevant elements can be found</param>
        /// <param name="hostedClaimsPrincipal">The claims principal settings to load into the claims</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddAirslipHostedAuth(
            this IServiceCollection services,
            IConfiguration configuration,
            HostedClaimsPrincipal hostedClaimsPrincipal)
        {
            services.AddOptions()
                .Configure<EnvironmentSettings>(configuration.GetSection(nameof(EnvironmentSettings)))
                .AddScoped<IClaimsPrincipalLocator>(_ => new HostedContextPrincipalLocator(hostedClaimsPrincipal))
                .AddScoped<IHttpContentLocator, HostedContextHeaderLocator>()
                .AddScoped<ITokenDecodeService<UserToken>, TokenDecodeService<UserToken>>();
            
            AirslipSchemeOptions.ThisEnvironment = services.GetEnvironment();

            return services;
        }
    }
}