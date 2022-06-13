using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.MongoDb
{
    public static class Services
    {
        public static MongoDbInitialisation AddAirslipMongoDb<TContext>(this IServiceCollection services, 
            IConfiguration config, Func<IMongoDatabase, Task> initialiseDatabase)
        where TContext : AirslipMongoDbBase
        {
            services
                .Configure<MongoDbSettings>(config.GetSection(nameof(MongoDbSettings)))
                .AddSingleton(Helpers
                    .InitializeMongoClientInstanceAsync(config, initialiseDatabase)
                    .GetAwaiter()
                    .GetResult())
                .AddSingleton<TContext>()
                .AddSingleton<IContext>(provider => provider.GetService<TContext>()!);

            return new MongoDbInitialisation(services, config);
        }
        
        public static IServiceCollection UseSearch<TContext>(this MongoDbInitialisation initialisation)
            where TContext : AirslipMongoDbSearchBase
        {
            initialisation.services
                .AddScoped<TContext>()
                .AddSingleton<ISearchContext>(provider => provider.GetService<TContext>()!);

            return initialisation.services;
        }
    }

    public record MongoDbInitialisation(IServiceCollection services, IConfiguration config);
}
