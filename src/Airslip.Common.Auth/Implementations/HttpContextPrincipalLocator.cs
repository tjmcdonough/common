using Airslip.Common.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Airslip.Common.Auth.Implementations
{
    public class HttpContextPrincipalLocator : IClaimsPrincipalLocator
    {
        private readonly HttpContext _httpContext;

        public HttpContextPrincipalLocator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext!;
        }
        
        public ClaimsPrincipal GetCurrentPrincipal()
        {
            return _httpContext.User;
        }
    }
}