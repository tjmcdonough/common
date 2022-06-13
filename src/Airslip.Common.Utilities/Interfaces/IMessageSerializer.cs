namespace Airslip.Common.Utilities.Interfaces
{
    public interface IMessageSerializer
    {
        SentMessage Serialize<TType>(TType serializeMe, string? withCorrelationId = null);
        
        ReceivedMessage<TType> Deserialize<TType>(byte[] deserializeMe);
    }

    public record SentMessage(byte[] RawMessage, string CorrelationId);

    public record ReceivedMessage<TType>(long Timestamp, string CorrelationId, TType Content);
}