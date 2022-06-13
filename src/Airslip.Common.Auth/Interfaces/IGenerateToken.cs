using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Enums;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Interfaces
{
    public interface IGenerateToken
    {
        string EntityId { get; init; }
        AirslipUserType AirslipUserType { get; init; }
        List<Claim> GetCustomClaims(TokenEncryptionSettings settings);
    }
}