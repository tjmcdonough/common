using Airslip.Common.Auth.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Airslip.Common.Auth.Implementations
{
    public class HttpContextContentLocator : IHttpContentLocator
    {
        private readonly HttpContext? _httpContext;

        public HttpContextContentLocator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }
        
        public string? GetHeaderValue(string headerValue, string? defaultValue = null)
        {
            if (!(_httpContext?.Request.Headers.ContainsKey(headerValue) ?? false)) 
                return defaultValue;
            
            return _httpContext!.Request.Headers[headerValue].ToString();
        }

        public string? GetQueryValue(string queryValue, string? defaultValue = null)
        {
            if (!(_httpContext?.Request.Query.ContainsKey(queryValue) ?? false)) 
                return defaultValue;
            
            return _httpContext!.Request.Query[queryValue].ToString();
        }
    }
}