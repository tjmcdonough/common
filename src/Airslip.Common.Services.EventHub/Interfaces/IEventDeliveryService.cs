using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Services.EventHub.Interfaces
{
    public interface IEventDeliveryService<TType>
        where TType : class, IFromDataSource
    {
        Task DeliverEvents(ICollection<TType> events, DataSources dataSource);
        Task DeliverEvents(TType thisEvent, DataSources dataSource);
    }
}