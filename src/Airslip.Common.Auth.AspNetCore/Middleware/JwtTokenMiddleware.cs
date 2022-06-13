using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.AspNetCore.Middleware
{
    public class JwtTokenMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public JwtTokenMiddleware(RequestDelegate next,  
            ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext, ITokenDecodeService<UserToken> tokenDecodeService)
        {
            try
            {
                IIdentity? identity = httpContext.User.Identity;
                
                if (!(identity?.IsAuthenticated ?? false))
                {
                    AuthenticateResult authenticateResult = await httpContext
                        .AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

                    if (authenticateResult.Succeeded && authenticateResult.Principal != null)
                    {
                        httpContext.User = authenticateResult.Principal;

                        UserToken userToken = tokenDecodeService.GetCurrentToken();
                        httpContext.Items["UserToken"] = userToken;
                    }
                }
                
                if (identity?.IsAuthenticated ?? false)
                {
                    UserToken userToken = tokenDecodeService.GetCurrentToken();
                    httpContext.Items["UserToken"] = userToken;
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "An unhandled authentication error occurred");
            }
                
            await _next(httpContext);
        }
    }
}