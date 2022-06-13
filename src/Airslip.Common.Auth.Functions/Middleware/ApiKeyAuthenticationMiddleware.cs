using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Functions.Middleware;

public class ApiKeyAuthenticationMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        // This is all wrapped in a try catch as the middleware executes
        //  for all types of functions... It's a bit naff!!
        try
        {
            HttpRequestData httpRequestData = context.GetHttpRequestData() ?? throw new NotImplementedException();

            IApiRequestAuthService requestHandler = context
                .InstanceServices
                .GetService<IApiRequestAuthService>() ?? throw new NotImplementedException();

            KeyAuthenticationResult authenticationResult = await requestHandler.Handle(context.FunctionDefinition.Name, 
                httpRequestData);

            context.Items.Add("authresult", authenticationResult);
        }
        catch (Exception)
        {
            context.Items.Add("authresult", new KeyAuthenticationResult()
            {
                AuthResult = AuthResult.NoResult
            });
        }
       
        await next(context);
    }
}