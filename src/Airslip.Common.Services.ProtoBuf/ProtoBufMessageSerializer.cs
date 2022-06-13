using Airslip.Common.Utilities;
using Airslip.Common.Utilities.Extensions;
using Airslip.Common.Utilities.Interfaces;
using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.IO;
using System.Linq;

namespace Airslip.Common.Services.ProtoBuf
{
    public class ProtoBufMessageSerializer : IMessageSerializer
    {
        internal static ProtoBufMessageSerializerOptions Options { get; set; } = new();
        private readonly RuntimeTypeModel _serializer;

        public ProtoBufMessageSerializer()
        {
            _serializer = RuntimeTypeModel.Create();

            foreach (var descriptor in Options.Descriptors)
            {
                descriptor.Register(_serializer);
            }
            
            _serializer.CompileInPlace();
        }

        public SentMessage Serialize<TType>(TType serializeMe, string? withCorrelationId = null)
        {
            using MemoryStream commandStream = new();
            _serializer.Serialize(commandStream, serializeMe);

            byte[] serializedCommand = commandStream.ToArray();
            MetaType? metaType = _serializer.GetTypes().Cast<MetaType>()
                .FirstOrDefault(meta => serializeMe != null && meta.Type == serializeMe.GetType());

            if (metaType is null)
                throw new ArgumentException("Unable to find MetaType", nameof(serializeMe));

            string typeUrl = metaType.Name;
            withCorrelationId ??= CommonFunctions.GetId();
            
            ProtobufEnvelope envelope = new(
                DateTime.UtcNow.ToUnixTimeMilliseconds(),
                withCorrelationId,
                new Any(
                    typeUrl,
                    serializedCommand
                ));

            using MemoryStream envelopeStream = new();
            _serializer.Serialize(envelopeStream, envelope);

            return new SentMessage(envelopeStream.ToArray(), withCorrelationId);
        }

        public ReceivedMessage<TType> Deserialize<TType>(byte[] deserializeMe)
        {
            ProtobufEnvelope envelope = _serializer
                .Deserialize<ProtobufEnvelope>(deserializeMe.AsSpan());

            MetaType? metaType = _serializer.GetTypes().Cast<MetaType>()
                .FirstOrDefault(meta => meta.Name == envelope.Content.TypeUrl);

            if (metaType is null)
                throw new ArgumentException(nameof(deserializeMe));

            Type commandType = metaType.Type;
            TType message =
                (TType)_serializer.Deserialize(commandType, envelope.Content.Value.AsSpan());

            return new ReceivedMessage<TType>(envelope.Timestamp, envelope.CorrelationId, message);
        }
        
        [ProtoContract(SkipConstructor = true)]
        [CompatibilityLevel(CompatibilityLevel.Level300)]
        private record ProtobufEnvelope(
            [property: ProtoMember(1)] long Timestamp,
            [property: ProtoMember(2)] string CorrelationId,
            [property: ProtoMember(3)] Any Content);
        
        [ProtoContract(SkipConstructor = true)]
        private record Any (
            [property: ProtoMember(1)] string TypeUrl,
            [property: ProtoMember(2)] byte[] Value);
    }
}