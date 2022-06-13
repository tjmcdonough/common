using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Functions.Configuration;
using Airslip.Common.Auth.Functions.Data;
using Airslip.Common.Auth.Functions.Implementations;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.Extensions.DependencyInjection;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Airslip.Common.Auth.Functions.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ServiceExtensions
    {
        /// <summary>
        /// Add ApiKey Validation for use in Function Apps
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The curren configuration</param>
        /// <param name="withEnvironment">The name of the environment which will be used for validating API Keys</param>
        /// <returns>The updated service collection</returns>
        public static ApiAccessOptions AddAirslipFunctionAuth(this IServiceCollection services, 
            IConfiguration configuration, string? withEnvironment = null)
        {
            ApiAccessRights apiAccessRights = new();
            
            services
                .Configure<TokenEncryptionSettings>(configuration.GetSection(nameof(TokenEncryptionSettings)))
                .AddScoped<IApiRequestAuthService, ApiRequestAuthService>()
                .AddScoped<IApiKeyRequestDataHandler, ApiKeyRequestDataHandler>()
                .AddScoped<IFunctionContextAccessor, FunctionContextAccessor>()
                .AddScoped<IClaimsPrincipalLocator, FunctionContextPrincipalLocator>()
                .AddScoped<IHttpContentLocator, FunctionContextHeaderLocator>()
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
                .Configure<EnvironmentSettings>(configuration.GetSection(nameof(EnvironmentSettings)))
                .AddScoped<ITokenDecodeService<ApiKeyToken>, TokenDecodeService<ApiKeyToken>>()
                .AddScoped<ITokenValidator<ApiKeyToken>, TokenValidator<ApiKeyToken>>()
                .AddScoped<IUserContext, ApiKeyTokenUserService>()
                .AddSingleton<IApiAccessRights>(apiAccessRights);
            AirslipSchemeOptions.ThisEnvironment = services.GetEnvironment();

            apiAccessRights.AddFromSettings(configuration);

            return new ApiAccessOptions(services, apiAccessRights);
        }
      
        internal static bool ValidateAccess(this ApiAccessDefinition accessDefinition, ApiKeyToken token)
        {
            bool allowedUserType = accessDefinition.AllowedTypes.Count == 0 || accessDefinition
                .AllowedTypes.Contains(token.AirslipUserType);
            bool allowedEntity = accessDefinition.AllowedEntities.Count == 0 || accessDefinition
                .AllowedEntities.Contains(token.EntityId);

            return allowedUserType && allowedEntity;
        }
    }
}