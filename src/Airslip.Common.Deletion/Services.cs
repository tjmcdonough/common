using Airslip.Common.Deletion.Interfaces;
using Airslip.Common.Deletion.Models;
using Airslip.Common.Deletion.Validation;
using Airslip.Common.Repository.Types.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Deletion;

public static class Services
{
    private static bool Initialised;
    
    public static IServiceCollection UseDeletion<TDeletionService>(
        this IServiceCollection services)
    where TDeletionService : class, IDeletionService
    {
        services
            .AddScoped<IDeletionService, TDeletionService>();
        return AddDeletionValidators(services);
    }
    
    public static IServiceCollection UseTypedDeletion<TModel, TDeletionService>(
        this IServiceCollection services)
        where TDeletionService : class, ITypedDeletionService<TModel> 
        where TModel : class, IModel
    {
        services
            .AddScoped<ITypedDeletionService<TModel>, TDeletionService>();
        
        return AddDeletionValidators(services);
    }
    
    private static IServiceCollection AddDeletionValidators(
        this IServiceCollection services)
    {
        if (Initialised) return services;
        
        services
            .AddScoped<IModelValidator<DeleteRequest>, DeletionValidator>();
        Initialised = true;

        return services;
    }
}