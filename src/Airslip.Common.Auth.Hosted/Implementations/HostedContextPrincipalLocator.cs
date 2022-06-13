using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Hosted.Implementations
{
    public class HostedContextPrincipalLocator : IClaimsPrincipalLocator
    {
        private readonly HostedClaimsPrincipal _hostedClaimsPrincipal;

        public HostedContextPrincipalLocator(HostedClaimsPrincipal hostedClaimsPrincipal)
        {
            _hostedClaimsPrincipal = hostedClaimsPrincipal;
        }

        public ClaimsPrincipal GetCurrentPrincipal()
        {
            List<Claim> claims = new()
            {
                new Claim(AirslipClaimTypes.CORRELATION,  
                    string.IsNullOrWhiteSpace(_hostedClaimsPrincipal.CorrelationId) ? CommonFunctions.GetId() : 
                        _hostedClaimsPrincipal.CorrelationId),
                new Claim(AirslipClaimTypes.AIRSLIP_USER_TYPE, AirslipUserType.InternalApi.ToString()),
                new Claim(AirslipClaimTypes.IP_ADDRESS, _hostedClaimsPrincipal.IpAddress),
                new Claim(AirslipClaimTypes.ENTITY_ID, _hostedClaimsPrincipal.EntityId),
                new Claim(AirslipClaimTypes.ENVIRONMENT, AirslipSchemeOptions.ThisEnvironment),
                new Claim(AirslipClaimTypes.USER_AGENT, _hostedClaimsPrincipal.UserAgent)
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }
    }
}