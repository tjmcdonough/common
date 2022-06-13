namespace Airslip.Common.Utilities.Interfaces
{
    public interface IProtobufSerializer<T>
    {
        byte[] Serialize(T value);
        T Deserialize(byte[] value);
    }
}