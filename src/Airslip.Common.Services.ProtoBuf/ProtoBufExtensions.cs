using Airslip.Common.Utilities.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Airslip.Common.Services.ProtoBuf
{
    public static class ProtoBufExtensions {
        public static IServiceCollection AddAirslipProtoBuf(this IServiceCollection services, Func<ProtoBufMessageSerializerOptions>? withOptions = null)
        {
            services
                .AddSingleton<IMessageSerializer, ProtoBufMessageSerializer>();
            ProtoBufMessageSerializer.Options = withOptions?.Invoke() ?? new ProtoBufMessageSerializerOptions();

            return services;
        }
    }
}