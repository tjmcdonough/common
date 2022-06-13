using Airslip.Common.Auth.AspNetCore.Attributes;
using Airslip.Common.Auth.AspNetCore.Configuration;
using Airslip.Common.Auth.AspNetCore.Handlers;
using Airslip.Common.Auth.AspNetCore.Implementations;
using Airslip.Common.Auth.AspNetCore.Interfaces;
using Airslip.Common.Auth.AspNetCore.Schemes;
using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Utilities.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Airslip.Common.Auth.AspNetCore.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Add token generation for the specified token type.
        /// Assumes application settings are available in the format of:
        /// 
        /// appSettings.json:
        /// {
        ///     "JwtSettings": {
        ///         "Key": "Example key",
        ///         "Issuer": "Example issuer",
        ///         "Audience": "Example audience",
        ///         "ExpiresTime": "Example expiry time",
        ///         "ValidateLifetime": "true"
        ///      }
        /// }
        /// 
        /// Environment Variables:
        /// JwtSettings:Key = Example key
        /// JwtSettings:Issuer = Example issuer
        /// JwtSettings:Audience = Example audience
        /// JwtSettings:ExpiresTime = Example expiry time
        /// JwtSettings:ValidateLifetime = true
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The primary configuration where relevant elements can be found</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddTokenGeneration<TTokenType>(this IServiceCollection services, 
            IConfiguration configuration) 
            where TTokenType : IGenerateToken
        {
            services
                .Configure<TokenEncryptionSettings>(configuration.GetSection(nameof(TokenEncryptionSettings)))
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
                .AddScoped<ITokenGenerationService<TTokenType>, TokenGenerationService<TTokenType>>();

            return services;
        }
        
        /// <summary>
        /// Add standard JWT authentication used across the Airslip API estate.
        /// Assumes application settings are available in the format of:
        /// 
        /// appSettings.json:
        /// {
        ///     "JwtSettings": {
        ///         "Key": "Example key",
        ///         "Issuer": "Example issuer",
        ///         "Audience": "Example audience",
        ///         "ExpiresTime": "Example expiry time",
        ///         "ValidateLifetime": "true"
        ///      }
        /// }
        /// 
        /// Environment Variables:
        /// JwtSettings:Key = Example key
        /// JwtSettings:Issuer = Example issuer
        /// JwtSettings:Audience = Example audience
        /// JwtSettings:ExpiresTime = Example expiry time
        /// JwtSettings:ValidateLifetime = true
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The primary configuration where relevant elements can be found</param>
        /// <param name="authType">The auth type that is supported by this Api</param>
        /// <param name="withEnvironment">The name of the environment which will be used for validating API Keys</param>
        /// <returns>The updated service collection</returns>
        public static AuthenticationBuilder? AddAirslipJwtAuth(this IServiceCollection services,  
            IConfiguration configuration, AuthType authType = AuthType.User, string? withEnvironment = null)
        {
            AuthenticationBuilder? result = null;

            services
                .AddScoped<IRemoteIpAddressService, RemoteIpAddressService>()
                .AddScoped<IUserAgentService, UserAgentService>()
                .AddScoped<IClaimsPrincipalLocator, HttpContextPrincipalLocator>()
                .AddScoped<IHttpContentLocator, HttpContextContentLocator>()
                .Configure<TokenEncryptionSettings>(configuration.GetSection(nameof(TokenEncryptionSettings)))
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
                .Configure<EnvironmentSettings>(configuration.GetSection(nameof(EnvironmentSettings)))
                .AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>()
                .AddAuthorization();

            if (authType.InList(AuthType.User, AuthType.All))
            {
                result = services
                    .AddScoped<ITokenDecodeService<UserToken>, TokenDecodeService<UserToken>>()
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();
            }
            
            if (authType.InList(AuthType.ApiKey, AuthType.All))
            {
                result = services
                    .AddScoped<IApiKeyRequestHandler, ApiKeyRequestHandler>()
                    .AddScoped<ITokenDecodeService<ApiKeyToken>, CryptoTokenDecodeService<ApiKeyToken>>()
                    .AddScoped<ITokenValidator<ApiKeyToken>, TokenValidator<ApiKeyToken>>()
                    .AddAuthentication(ApiKeyAuthenticationSchemeOptions.ApiKeyScheme)
                    .AddApiKeyAuth(opt =>
                    {
                        opt.Environment = withEnvironment ?? services.GetEnvironment();
                    });
            }

            return result;
        }

        /// <summary>
        /// Add QR Code authentication.
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The primary configuration where relevant elements can be found</param>
        /// <param name="withEnvironment">The name of the environment which will be used for validating QR Codes</param>
        /// <returns>The updated service collection</returns>
        public static AuthenticationBuilder AddAirslipQrCodeAuth(this IServiceCollection services, 
            IConfiguration configuration, string? withEnvironment)
        {
            services
                .AddSingleton<IQrCodeRequestHandler, QrCodeRequestHandler>()
                .AddAuthorization()
                .Configure<TokenEncryptionSettings>(configuration.GetSection(nameof(TokenEncryptionSettings)))
                .Configure<EnvironmentSettings>(configuration.GetSection(nameof(EnvironmentSettings)))
                .AddScoped<ITokenDecodeService<QrCodeToken>, CryptoTokenDecodeService<QrCodeToken>>()
                .AddScoped<ITokenValidator<QrCodeToken>, TokenValidator<QrCodeToken>>();
            
            AuthenticationBuilder result = services
                .AddAuthentication(QrCodeAuthenticationSchemeOptions.QrCodeAuthScheme)
                .AddQrCodeAuth(opt =>
                {
                    opt.Environment = withEnvironment ?? services.GetEnvironment();
                });

            return result;
        }
        
        
        /// <summary>
        /// Adds API access validation for use in microservice communication
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration used to load settings</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddApiAccessValidation(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services
                .TryAddScoped<IApiKeyRequestHandler, ApiKeyRequestHandler>();
            
            services.TryAddScoped<ITokenValidator<ApiKeyToken>, TokenValidator<ApiKeyToken>>();
            services.TryAddScoped<ITokenDecodeService<ApiKeyToken>, CryptoTokenDecodeService<ApiKeyToken>>();
            services.TryAddScoped<IRemoteIpAddressService, RemoteIpAddressService>();
            services.TryAddScoped<IUserAgentService, UserAgentService>();
            services.TryAddScoped<IClaimsPrincipalLocator, HttpContextPrincipalLocator>();
            services.TryAddScoped<IHttpContentLocator, HttpContextContentLocator>();
                
            services
                .AddScoped<IApiRequestAuthService, ApiRequestAuthService>()
                .Configure<TokenEncryptionSettings>(configuration.GetSection(nameof(TokenEncryptionSettings)))
                .Configure<ApiAccessSettings>(configuration.GetSection(nameof(ApiAccessSettings)));
            
            return services;
        }
        
        public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, 
            IConfiguration configuration, string? withEnvironment)
        {
            services.TryAddScoped<ICookieRequestHandler, CookieRequestHandler>();
            services.TryAddScoped<ICookieService, CookieService>();
            services.TryAddScoped<ITokenValidator<UserToken>, TokenValidator<UserToken>>();
            services.TryAddScoped<ITokenDecodeService<UserToken>, TokenDecodeService<UserToken>>();
            services.TryAddScoped<ITokenGenerationService<GenerateUserToken>, TokenGenerationService<GenerateUserToken>>();
            services.TryAddScoped<IRemoteIpAddressService, RemoteIpAddressService>();
            services.TryAddScoped<IUserAgentService, UserAgentService>();
            services.TryAddScoped<IClaimsPrincipalLocator, HttpContextPrincipalLocator>();
            services.TryAddScoped<IHttpContentLocator, HttpContextContentLocator>();
                
            services
                .Configure<TokenEncryptionSettings>(configuration.GetSection(nameof(TokenEncryptionSettings)))
                .Configure<CookieSettings>(configuration.GetSection(nameof(CookieSettings)));
            
            AuthenticationBuilder result = services
                .AddAuthentication(CookieAuthenticationSchemeOptions.CookieAuthScheme)
                .AddCookieAuth(opt =>
                {
                    opt.Environment = withEnvironment ?? services.GetEnvironment();
                });
            
            return services;
        }
        
        public static AuthenticationBuilder AddApiKeyAuth(this AuthenticationBuilder builder, Action<ApiKeyAuthenticationSchemeOptions> options)
        {
            return builder
                .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthHandler>(ApiKeyAuthenticationSchemeOptions.ApiKeyScheme, options);
        }
        
        public static AuthenticationBuilder AddCookieAuth(this AuthenticationBuilder builder, Action<CookieAuthenticationSchemeOptions> options)
        {
            return builder
                .AddScheme<CookieAuthenticationSchemeOptions, CookieAuthHandler>(CookieAuthenticationSchemeOptions.CookieAuthScheme, options);
        }
        
        public static AuthenticationBuilder AddQrCodeAuth(this AuthenticationBuilder builder, Action<QrCodeAuthenticationSchemeOptions> options)
        {
            return builder
                .AddScheme<QrCodeAuthenticationSchemeOptions, QrCodeAuthHandler>(QrCodeAuthenticationSchemeOptions.QrCodeAuthScheme, options);
        }
    }
}