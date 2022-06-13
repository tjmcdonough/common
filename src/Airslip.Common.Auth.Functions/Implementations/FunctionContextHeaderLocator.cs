using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Interfaces;
using System.Linq;

namespace Airslip.Common.Auth.Functions.Implementations
{
    public class FunctionContextHeaderLocator : IHttpContentLocator
    {
        private readonly IFunctionContextAccessor _functionContextAccessor;

        public FunctionContextHeaderLocator(IFunctionContextAccessor functionContextAccessor)
        {
            _functionContextAccessor = functionContextAccessor;
        }
        
        public string? GetHeaderValue(string headerValue, string? defaultValue = null)
        {
            if (!(_functionContextAccessor.Headers?.Contains(headerValue) ?? false)) 
                return defaultValue;
            
            return _functionContextAccessor.Headers!.GetValues(headerValue).First();
        }

        public string? GetQueryValue(string queryValue, string? defaultValue = null)
        {
            if (!(_functionContextAccessor.QueryString?.AllKeys.Contains(queryValue) ?? false)) 
                return defaultValue;
            
            return _functionContextAccessor.QueryString!.Get(queryValue);
        }
    }
}