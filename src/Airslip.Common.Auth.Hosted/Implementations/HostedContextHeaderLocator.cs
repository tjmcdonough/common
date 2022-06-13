using Airslip.Common.Auth.Interfaces;

namespace Airslip.Common.Auth.Hosted.Implementations
{
    public class HostedContextHeaderLocator : IHttpContentLocator
    {
        public string? GetHeaderValue(string headerValue, string? defaultValue = null)
        {
            return string.Empty;
        }

        public string? GetQueryValue(string queryValue, string? defaultValue = null)
        {
            return string.Empty;
        }
    }
}