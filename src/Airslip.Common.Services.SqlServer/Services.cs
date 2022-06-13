using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.SqlServer.Implementations;
using Airslip.Common.Services.SqlServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Services.SqlServer
{
    public static class Services
    {
        public static IServiceCollection AddAirslipSqlServer<TContext>(this IServiceCollection services, 
            IConfiguration config)
        where TContext : AirslipSqlServerContextBase
        {
            services.AddDbContext<TContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(config.GetConnectionString("SqlServer")));
            
            services
                .AddSingleton<IQueryBuilder, QueryBuilder>()
                .AddScoped<IContext>(provider => provider.GetService<TContext>()!)
                .AddScoped<ISearchContext>(provider => provider.GetService<TContext>()!);

            return services;
        }
    }
}
