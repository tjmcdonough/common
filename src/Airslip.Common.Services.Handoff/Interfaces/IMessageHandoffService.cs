using Airslip.Common.Types.Enums;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Handoff.Interfaces;

public interface IMessageHandoffService
{
    Task ProcessMessage(string triggerName, string message);
    Task ProcessMessage<TImplementation>(string triggerName, string message)
        where TImplementation : IMessageHandoffWorker;
}