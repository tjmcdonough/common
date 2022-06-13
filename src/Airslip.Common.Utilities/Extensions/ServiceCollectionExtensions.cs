using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Linq;

namespace Airslip.Common.Utilities.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Remove<TServiceType>(this IServiceCollection services)
    {
        if (services.IsReadOnly)
        {
            throw new ReadOnlyException($"{nameof(services)} is read only");
        }

        ServiceDescriptor? serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(TServiceType));
        if (serviceDescriptor != null) services.Remove(serviceDescriptor);

        return services;
    }
    
    public static IServiceCollection ReplaceScoped<TServiceType, TWithService>(this IServiceCollection services)
        where TWithService : class, TServiceType where TServiceType : class
    {
        services.Remove<TServiceType>();
        services.AddScoped<TServiceType, TWithService>();
        return services;
    }
    
    public static IServiceCollection ReplaceSingleton<TServiceType, TWithService>(this IServiceCollection services)
        where TWithService : class, TServiceType where TServiceType : class
    {
        services.Remove<TServiceType>();
        services.AddSingleton<TServiceType, TWithService>();
        return services;
    }
    
    public static IServiceCollection ReplaceTransient<TServiceType, TWithService>(this IServiceCollection services)
        where TWithService : class, TServiceType where TServiceType : class
    {
        services.Remove<TServiceType>();
        services.AddTransient<TServiceType, TWithService>();
        return services;
    }
}