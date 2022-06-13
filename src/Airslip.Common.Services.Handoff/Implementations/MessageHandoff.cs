using Airslip.Common.Types.Enums;
using System;

namespace Airslip.Common.Services.Handoff.Implementations;

public class MessageHandoff
{
    public MessageHandoff(Type handlerType, string queueName)
    {
        HandlerType = handlerType;
        QueueName = queueName;
    }

    public Type HandlerType { get; init; }
    public string QueueName { get; init; }
}