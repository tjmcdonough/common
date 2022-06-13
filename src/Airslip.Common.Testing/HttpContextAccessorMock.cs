using Airslip.Common.Auth.Data;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Airslip.Common.Testing
{
    public static class HttpContextAccessorMock
    {
        public static Mock<IHttpContextAccessor> GenerateContext(
            string token,
            TokenTypeMock tokenTypeMock = TokenTypeMock.BearerToken,
            string userAgent = CommonAgents.UA_WINDOWS_10_EDGE,
            string? forwarder = null, 
            string? remoteAddr = null,
            ClaimsPrincipal? withClaimsPrincipal = null)
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            DefaultHttpContext context = new();
            
            switch (tokenTypeMock)
            {
                case TokenTypeMock.BearerToken:
                    context.Request.Headers["Authorization"] = $"Bearer {token}";
                    break;
                case TokenTypeMock.QrCode:
                    context.Request.QueryString = QueryString.FromUriComponent($"?{token}");
                    break;
                case TokenTypeMock.ApiKey:
                    context.Request.Headers[AirslipSchemeOptions.ApiKeyHeaderField] = token;
                    break;
            }

            if (forwarder != null) context.Request.Headers["X-Forwarded-For"] = forwarder;
            if (remoteAddr != null) context.Request.Headers["REMOTE_ADDR"] = remoteAddr;
            if (withClaimsPrincipal != null) context.User = withClaimsPrincipal;
            
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            
            return mockHttpContextAccessor;
        }
    }
}