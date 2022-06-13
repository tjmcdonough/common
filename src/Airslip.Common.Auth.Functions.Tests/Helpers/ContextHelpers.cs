using Airslip.Common.Auth.Data;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Airslip.Common.Auth.Functions.Tests.Helpers
{
    public static class ContextHelpers
    {
        public static Mock<IHttpContextAccessor> GenerateContext(
            string token,
            TokenType tokenType = TokenType.BearerToken,
            string userAgent = Constants.UA_WINDOWS_10_EDGE,
            string forwarder = null, 
            string remoteAddr = null,
            ClaimsPrincipal withClaimsPrincipal = null)
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            DefaultHttpContext context = new();
            
            switch (tokenType)
            {
                case TokenType.BearerToken:
                    context.Request.Headers["Authorization"] = $"Bearer {token}";
                    break;
                case TokenType.QrCode:
                    context.Request.QueryString = QueryString.FromUriComponent($"?{token}");
                    break;
                case TokenType.ApiKey:
                    context.Request.Headers[AirslipSchemeOptions.ApiKeyHeaderField] = token;
                    break;
            }

            if (userAgent != null) context.Request.Headers["User-Agent"] = userAgent;
            if (forwarder != null) context.Request.Headers["X-Forwarded-For"] = forwarder;
            if (remoteAddr != null) context.Request.Headers["REMOTE_ADDR"] = remoteAddr;
            if (withClaimsPrincipal != null) context.User = withClaimsPrincipal;
            
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            
            return mockHttpContextAccessor;
        }
    }

    public enum TokenType
    {
        BearerToken,
        QrCode,
        ApiKey
    }
}