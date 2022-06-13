namespace Airslip.Common.Auth.Interfaces
{
    public interface IHttpContentLocator
    {
        string? GetHeaderValue(string headerValue, string? defaultValue = null);
        string? GetQueryValue(string queryValue, string? defaultValue = null);
    }
}