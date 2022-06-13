using Airslip.Common.Types.Enums;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Handoff.Interfaces;

public interface IMessageHandoffWorker
{
    Task Execute(string message);
}