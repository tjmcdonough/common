using Airslip.Common.Services.Handoff.Data;
using Airslip.Common.Services.Handoff.Implementations;
using Airslip.Common.Services.Handoff.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Airslip.Common.Services.Handoff;

public static class Services
{
    public static IServiceCollection UseMessageHandoff(this IServiceCollection services, 
        Action<MessageHandoffOptions> initialise)
    {
        MessageHandoffOptions messageHandoff = new();
        services.AddScoped<IMessageHandoffService, MessageHandoffService>();
        initialise(messageHandoff);
        return services;
    }
}