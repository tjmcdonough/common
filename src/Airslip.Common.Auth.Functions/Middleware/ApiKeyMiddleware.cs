using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Airslip.Common.Auth.Functions.Middleware;

internal static class FunctionContextExtensions
{
    internal static HttpRequestData? GetHttpRequestData(this FunctionContext functionContext)
    {
        try
        {
            KeyValuePair<Type, object> keyValuePair = functionContext
                .Features
                .SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
            object functionBindingsFeature = keyValuePair.Value;
            Type type = functionBindingsFeature.GetType();
            IReadOnlyDictionary<string, object>? inputData = type
                .GetProperty("InputData")!
                .GetValue(functionBindingsFeature) as IReadOnlyDictionary<string, object>;
            return inputData?.Values.SingleOrDefault(o => o is HttpRequestData) as HttpRequestData;
        }
        catch
        {
            return null;
        }
    }

    internal static List<T> GetCustomAttributesOnClassAndMethod<T>(this MethodInfo targetMethod)
        where T : Attribute
    {
        IEnumerable<T> methodAttributes = targetMethod.GetCustomAttributes<T>();
        Type declaringType = targetMethod.DeclaringType ?? throw new NotImplementedException();
        IEnumerable<T> classAttributes = declaringType.GetCustomAttributes<T>();
        return methodAttributes.Concat(classAttributes).ToList();
    }
    
    internal static MethodInfo GetTargetFunctionMethod(this FunctionContext context)
    {
        string entryPoint = context.FunctionDefinition.EntryPoint;

        string assemblyPath = context.FunctionDefinition.PathToAssembly;
        Assembly assembly = Assembly.LoadFrom(assemblyPath);
        string typeName = entryPoint.Substring(0, entryPoint.LastIndexOf('.'));
        Type type = assembly.GetType(typeName) ?? throw new NotImplementedException();
        string methodName = entryPoint.Substring(entryPoint.LastIndexOf('.') + 1);
        MethodInfo method = type.GetMethod(methodName) ?? throw new NotImplementedException();
        return method;
    }
    
    internal static void SetHttpResponseStatusCode(
        this FunctionContext context,
        HttpStatusCode statusCode)
    {
        KeyValuePair<Type, object> keyValuePair = context
            .Features
            .SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
        object functionBindingsFeature = keyValuePair.Value;
        Type type = functionBindingsFeature.GetType();
        PropertyInfo invocationResult = type
            .GetProperty("InvocationResult") ?? throw new NotImplementedException();;
        IReadOnlyDictionary<string, object>? inputData = type
            .GetProperty("InputData")!
            .GetValue(functionBindingsFeature) as IReadOnlyDictionary<string, object>;
        HttpRequestData req = inputData?.Values.SingleOrDefault(o => o is HttpRequestData) as HttpRequestData
                              ?? throw new NotImplementedException();;
        
        invocationResult
            .SetMethod!
            .Invoke(functionBindingsFeature, new object[] { req.CreateResponse(HttpStatusCode.Unauthorized) });
    }
}
