using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.CosmosDb.Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.CosmosDb
{
    public static class Services
    {
        public static CosmosDbInitialisation AddAirslipCosmosDb<TContext>(this IServiceCollection services,
            IConfiguration config, Func<Database, Task> initialiseCollections,
            ConnectionMode connectionMode = ConnectionMode.Direct,
            ConsistencyLevel consistencyLevel = ConsistencyLevel.Session)
            where TContext : AirslipCosmosDbBase
        {
            services
                .Configure<CosmosDbSettings>(config.GetSection(nameof(CosmosDbSettings)))
                .AddSingleton(Helpers
                    .InitializeCosmosClientInstanceAsync(config, initialiseCollections, connectionMode,
                        consistencyLevel)
                    .GetAwaiter()
                    .GetResult())
                .AddSingleton<TContext>()
                .AddSingleton<IContext>(provider => provider.GetService<TContext>()!);

            return new CosmosDbInitialisation(services, config);
        }

        public static IServiceCollection UseSearch<TContext>(this CosmosDbInitialisation initialisation)
            where TContext : AirslipCosmosDbSearchBase
        {
            initialisation.services
                .AddScoped<TContext>()
                .AddSingleton<ISearchContext>(provider => provider.GetService<TContext>()!);

            return initialisation.services;
        }
    }

    public record CosmosDbInitialisation(IServiceCollection services, IConfiguration config);
}