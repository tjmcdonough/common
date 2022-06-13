using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public class ApiKeyToken : TokenBase
    {
        public string UserId { get; private set; } = "";
        public string ApiKey { get; private set; } = "";
        public string UserRole { get; private set; } = UserRoles.User;
        public string[] ApplicationRoles { get; private set; } = {};
        
        public override void SetCustomClaims(List<Claim> tokenClaims, TokenEncryptionSettings settings)
        {
            ApiKey = tokenClaims.GetValue(AirslipClaimTypes.API_KEY_SHORT);
            UserId = tokenClaims.GetValue(AirslipClaimTypes.USER_ID_SHORT);
            UserRole = tokenClaims.GetValue(AirslipClaimTypes.USER_ROLE_SHORT);
            ApplicationRoles = tokenClaims.GetValue(AirslipClaimTypes.APPLCATION_ROLES_SHORT)
                .Split(";");
        }
    }
}