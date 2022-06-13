using Airslip.Common.Services.EventHub.Interfaces;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;
using System.Collections.Concurrent;

namespace Airslip.Common.Services.EventHub.Implementations
{
    public class EventHubFactory : IEventHubFactory
    {
        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<string, object> _hubs = new();
        private readonly IOptions<EventHubSettings> _options;

        public EventHubFactory(IOptions<EventHubSettings> options, ILogger logger)
        {
            _logger = logger;
            _options = options;
        }
        
        public IEventDeliveryService<TType> CreateInstance<TType>(string eventHubName) 
            where TType : class, IFromDataSource
        {
            string typeName = typeof(TType).FullName ?? string.Empty;
            
            if (!_hubs.ContainsKey(typeName))
            {
                _hubs[typeName] = new EventHubDeliveryService<TType>(_options, eventHubName, _logger);
            }

            return (IEventDeliveryService<TType>) _hubs[typeName];
        }
    }
}