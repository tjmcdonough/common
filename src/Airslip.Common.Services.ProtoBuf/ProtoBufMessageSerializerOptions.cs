using ProtoBuf.Meta;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Services.ProtoBuf
{
    public class ProtoBufMessageSerializerOptions
    {
        public const string SchemaNamespace = "type.googleapis.com/airslip";
        
        internal record Descriptor(
            Action<RuntimeTypeModel> Register);

        internal List<Descriptor> Descriptors { get; init; } = new();

        public void RegisterSerializeableType
            (Action<RuntimeTypeModel> generator)
        {
            Descriptors.Add(new Descriptor(generator));
        }
    }
}