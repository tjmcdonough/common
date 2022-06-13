using Airslip.Common.Services.EventHub.Interfaces;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities;
using Airslip.Common.Utilities.Extensions;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Services.EventHub.Implementations;

public class EventHubDeliveryService<TType> : IEventDeliveryService<TType> 
    where TType : class, IFromDataSource
{
    private readonly ILogger _logger;
    private readonly EventHubProducerClient _producerClient;   
        
    public EventHubDeliveryService(IOptions<EventHubSettings> options, string eventHubName, ILogger logger)
    {
        _logger = logger;
        EventHubSettings? eventHubSettings = options.Value;
        _producerClient = new EventHubProducerClient(eventHubSettings.ConnectionString, 
            eventHubName);
    }
        
    public async Task DeliverEvents(ICollection<TType> events, DataSources dataSource)
    {
        // Create a batch of events
        try
        {
            await _executeChunked(
                data => _producerClient.SendAsync(data),
                model => new EventData(Json.Serialize(model)),
                events, 
                10, 
                dataSource);
        }
        catch (Exception eee)
        {
            _logger.Fatal(eee, "Error sending events");
        }
    }

    public async Task DeliverEvents(TType thisEvent, DataSources dataSource)
    {
        await DeliverEvents(new[] { thisEvent }, dataSource);
    }

    private static async Task _executeChunked<TExecuteType>(Func<IEnumerable<TExecuteType>, Task> executeMe,
        Func<TType, TExecuteType> createMe,
        ICollection<TType> myCollection,
        int chunkSize, DataSources dataSource) 
    {
        // Create a batch of events
        List<TExecuteType> batchList = new();
        int batchCount = 0;
            
        foreach (TType model in myCollection)
        {
            model.DataSource = dataSource;
            model.TimeStamp = model.TimeStamp == 0 ? DateTime.UtcNow.ToUnixTimeMilliseconds() : model.TimeStamp;
            
            // Convert envelope to a string
            batchList.Add(createMe(model));
            batchCount += 1;

            if (batchCount < chunkSize) continue;
                
            await executeMe(batchList);

            batchList.Clear();
            batchCount = 0;
        }
            
        if  (batchList.Any()) 
            await executeMe(batchList);
    }
}