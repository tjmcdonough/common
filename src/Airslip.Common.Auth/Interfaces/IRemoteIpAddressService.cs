namespace Airslip.Common.Auth.Interfaces
{
    public interface IRemoteIpAddressService
    {
        string? GetRequestIP();
    }
}