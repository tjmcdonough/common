using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Services.EventHub.Interfaces
{
    public interface IEventHubFactory
    {
        IEventDeliveryService<TType> CreateInstance<TType>(string eventHubName) 
            where TType : class, IFromDataSource;
    }
}