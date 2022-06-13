using System.Security.Claims;

namespace Airslip.Common.Auth.Interfaces
{
    public interface IClaimsPrincipalLocator
    {
        ClaimsPrincipal? GetCurrentPrincipal();
    }
}