using Airslip.Common.Auth.Functions.Attributes;
using Airslip.Common.Auth.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Functions.Middleware;

public class ApiKeyAuthorisationMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        // This is all wrapped in a try catch as the middleware executes
        //  for all types of functions... It's a bit naff!!
        try
        {
            MethodInfo methodInfo = context.GetTargetFunctionMethod();
            bool requiresAuth = methodInfo.GetCustomAttributesOnClassAndMethod<ApiKeyAuthorizeAttribute>().Any();
            if (requiresAuth)
            {
                try
                {
                    KeyAuthenticationResult authResult = (KeyAuthenticationResult) context.Items["authresult"];
                    if (authResult.AuthResult != AuthResult.Success)
                    {
                        context.SetHttpResponseStatusCode(HttpStatusCode.Unauthorized);
                        return;
                    }

                }
                catch (Exception)
                {
                    context.SetHttpResponseStatusCode(HttpStatusCode.Unauthorized);
                }
            }
        }
        catch (Exception)
        {
            
        }
        
        await next(context);
    }
}