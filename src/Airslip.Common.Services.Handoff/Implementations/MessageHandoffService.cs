using Airslip.Common.Services.Handoff.Interfaces;
using Airslip.Common.Types.Enums;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Handoff.Implementations;

public class MessageHandoffService : IMessageHandoffService
{
    internal static readonly List<MessageHandoff> Handlers = new();
    private readonly IServiceProvider _provider;
    private readonly ILogger _logger;

    public MessageHandoffService(IServiceProvider provider, ILogger logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task ProcessMessage(string triggerName, string message)
    {
        IDisposable? logProperty = LogContext.PushProperty("CorrelationId", Guid.NewGuid().ToString());
        _logger.Debug("Triggered {TriggerName}", triggerName);

        if (!Handlers.Any(o => o.QueueName.Equals(triggerName)))
        {
            _logger.Fatal("Error in MessageHandoffService, handler not found for {TriggerName}", 
                triggerName);
            return;
        }
    
        try {
            MessageHandoff handler = Handlers
                .First(o => o.QueueName.Equals(triggerName));

            object? worker = _provider
                .GetService(handler.HandlerType);

            if (worker is not IMessageHandoffWorker messageHandoffWorker)
            {
                _logger.Fatal("Worker not found for {TriggerName}", 
                    triggerName);
                return;
            }
            
            await messageHandoffWorker.Execute(message);
        }
        catch (Exception ee)
        {
            _logger.Fatal(ee, "Uncaught error in {TriggerName}", triggerName);                
        }
        
        _logger.Debug("Completed {TriggerName}", triggerName);
        logProperty?.Dispose();
    }

    public async Task ProcessMessage<TImplementation>(string triggerName, string message) 
        where TImplementation : IMessageHandoffWorker
    {
        IDisposable? logProperty = LogContext.PushProperty("CorrelationId", Guid.NewGuid().ToString());
        _logger.Debug("Triggered {TriggerName}", triggerName);

        try {
            TImplementation? worker = _provider
                .GetService<TImplementation>();

            if (worker is not IMessageHandoffWorker messageHandoffWorker)
            {
                _logger.Fatal("Worker not found for {TriggerName}", 
                    triggerName);
                return;
            }
            
            await messageHandoffWorker.Execute(message);
        }
        catch (Exception ee)
        {
            _logger.Fatal(ee, "Uncaught error in {TriggerName}", triggerName);                
        }
        
        _logger.Debug("Completed {TriggerName}", triggerName);
        logProperty?.Dispose();
    }
}