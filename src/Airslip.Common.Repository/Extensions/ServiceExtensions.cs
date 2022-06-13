using Airslip.Common.Repository.Configuration;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations;
using Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;
using Airslip.Common.Repository.Implementations.Events.Entity.PreValidate;
using Airslip.Common.Repository.Implementations.Events.Model.PostProcess;
using Airslip.Common.Repository.Implementations.Events.Model.PreValidate;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Repository.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration,
        RepositoryUserType repositoryUserType = RepositoryUserType.TokenBased)
    {
        return ConfigureServices(services, configuration, repositoryUserType);
    }
    
    public static IServiceCollection AddEntitySearch(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEntitySearch<>), typeof(EntitySearch<>));

        return services;
    }
    
    private static IServiceCollection ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration,
        RepositoryUserType repositoryUserType)
    {
        serviceCollection
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .Configure<RepositorySettings>(configuration.GetSection(nameof(RepositorySettings)))
            .AddScoped<IRepositoryMetricService, RepositoryMetricService>()
            .AddScoped(typeof(IRepositoryLifecycle<,>), typeof(RepositoryLifecycle<,>))
            .AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
            .AddScoped(typeof(IModelPostProcessEvent<>), typeof(ModelDeliveryEvent<>))
            .AddScoped(typeof(IModelPostProcessEvent<>), typeof(ModelFormatEvent<>))
            .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityTimeStampEvent<>))
            .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityBasicAuditEvent<>))
            .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityOwnershipEvent<>))
            .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityStatusEvent<>))
            .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityDefaultIdEvent<>))
            .AddScoped(typeof(IEntityPreValidateEvent<,>), typeof(EntityFoundValidation<,>))
            .AddScoped(typeof(IEntityPreValidateEvent<,>), typeof(EntityStatusValidation<,>))
            .AddScoped(typeof(IEntityPreValidateEvent<,>), typeof(EntityTimelineValidation<,>))
            .AddScoped(typeof(IModelPreValidateEvent<,>), typeof(ModelCreateValidation<,>))
            .AddScoped(typeof(IModelPreValidateEvent<,>), typeof(ModelIdValidation<,>))
            .AddScoped(typeof(IModelPreValidateEvent<,>), typeof(ModelUpdateValidation<,>))
            .AddScoped(typeof(IModelPreValidateEvent<,>), typeof(IdRequiredValidation<,>));
            
        switch (repositoryUserType)
        {
            case RepositoryUserType.Null:
                serviceCollection.AddSingleton<IUserContext, NullUserContext>();
                break;
            case RepositoryUserType.TokenBased:
                serviceCollection
                    .AddScoped<IUserContext, TokenBasedUserContext>()
                    .AddScoped(typeof(IEntityPreValidateEvent<,>), 
                        typeof(EntityOwnershipValidationEvent<,>));
                break;
            case RepositoryUserType.Service:
                serviceCollection.AddScoped<IUserContext, ServiceUserContext>();
                break;
            case RepositoryUserType.Manual:
                // Inject nothing, let the developer do it...
                break;
        }

        return serviceCollection;
    }
}