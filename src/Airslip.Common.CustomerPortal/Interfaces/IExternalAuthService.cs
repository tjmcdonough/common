namespace Airslip.Common.CustomerPortal.Interfaces
{
    public interface IExternalAuthService
    {
        string GenerateCallbackUrl(string accountId, string? redirectUri = null);
    }
}