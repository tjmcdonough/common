using Airslip.Common.MerchantTransactions.Generated;
using Airslip.Common.MerchantTransactions.Implementations;
using Airslip.Common.MerchantTransactions.Interfaces;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Net.Http;

namespace Airslip.Common.MerchantTransactions.Extensions
{
    public static class MerchantTransactionsExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The primary configuration where relevant elements can be found</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddMerchantTransactions<TSource>(
            this IServiceCollection services,
            IConfiguration configuration)
        where TSource : class
        {
            services
                .Configure<PublicApiSettings>(configuration.GetSection(nameof(PublicApiSettings)))
                .AddScoped<IInternalApiV1Client>(provider =>
                {
                    IOptions<PublicApiSettings> apiSettings = provider.GetService<IOptions<PublicApiSettings>>()!;
                    IHttpClientFactory? httpClientFactory = provider.GetService<IHttpClientFactory>();

                    PublicApiSetting merchantTransactionsSettings =
                        apiSettings.Value.GetSettingByName("MerchantTransactions");

                    if (httpClientFactory == null)
                        throw new ArgumentException("httpClientFactory not found");

                    HttpClient httpClient = httpClientFactory
                        .CreateClient(nameof(InternalApiV1Client));

                    InternalApiV1Client client = new(httpClient)
                    {
                        BaseUrl = merchantTransactionsSettings.ToBaseUri()
                    };
                    client.SetApiKeyToken(merchantTransactionsSettings.ApiKey);

                    return client;
                })
                .AddScoped<IExternalApiV1Client>(provider =>
                {
                    IOptions<PublicApiSettings> apiSettings = provider.GetService<IOptions<PublicApiSettings>>()!;
                    IHttpClientFactory? httpClientFactory = provider.GetService<IHttpClientFactory>();

                    PublicApiSetting merchantTransactionsSettings =
                        apiSettings.Value.GetSettingByName("MerchantTransactions");

                    if (httpClientFactory == null)
                        throw new ArgumentException("httpClientFactory not found");

                    HttpClient httpClient = httpClientFactory
                        .CreateClient(nameof(ExternalApiV1Client));

                    ExternalApiV1Client client = new(httpClient)
                    {
                        BaseUrl = merchantTransactionsSettings.ToBaseUri()
                    };

                    return client;
                })
                .AddScoped<IMerchantIntegrationService<TSource>, MerchantIntegrationService<TSource>>();
            
            services
                .AddHttpClient<InternalApiV1Client>(nameof(InternalApiV1Client))
                .AddTransientHttpErrorPolicy(p =>
                    p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            services
                .AddHttpClient<ExternalApiV1Client>(nameof(ExternalApiV1Client))
                .AddTransientHttpErrorPolicy(p =>
                    p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            return services;
        }
    }
}