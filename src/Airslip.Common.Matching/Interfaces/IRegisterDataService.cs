using Airslip.Common.Services.Handoff.Interfaces;
using JetBrains.Annotations;

namespace Airslip.Common.Matching.Interfaces;

[UsedImplicitly]
public interface IRegisterDataService<in TType> : IMessageHandoffWorker 
    where TType : class
{
        
}