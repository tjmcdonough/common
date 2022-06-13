using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Interfaces;
using System.Security.Claims;

namespace Airslip.Common.Auth.Functions.Implementations
{
    public class FunctionContextPrincipalLocator : IClaimsPrincipalLocator
    {
        private readonly IFunctionContextAccessor _functionContextAccessor;

        public FunctionContextPrincipalLocator(IFunctionContextAccessor functionContextAccessor)
        {
            _functionContextAccessor = functionContextAccessor;
        }
        
        public ClaimsPrincipal? GetCurrentPrincipal()
        {
            return _functionContextAccessor.User;
        }
    }
}