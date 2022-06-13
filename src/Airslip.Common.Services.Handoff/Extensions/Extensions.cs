using Airslip.Common.Services.Handoff.Data;
using Airslip.Common.Services.Handoff.Implementations;
using Airslip.Common.Services.Handoff.Interfaces;
using Airslip.Common.Types.Enums;
using JetBrains.Annotations;

namespace Airslip.Common.Services.Handoff.Extensions;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class Extensions
{
    public static MessageHandoffOptions Register<THandoffProcessor>
        (this MessageHandoffOptions messageHandoff, string queueName)
        where THandoffProcessor : IMessageHandoffWorker
    {
        MessageHandoffService.Handlers.Add(
            new MessageHandoff(typeof(THandoffProcessor), queueName));

        return messageHandoff;
    }
}