using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Types.Enums;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public abstract class TokenBase : IDecodeToken
    {
        public string TokenType { get; init; } = "";
        public bool? IsAuthenticated { get; init; }
        public string CorrelationId { get; init; } = "";
        public string IpAddress { get; init; } = "";
        public string BearerToken { get; init; } = "";
        public string UserAgent { get; init; } = "";
        public string EntityId { get; init; } = "";
        public AirslipUserType AirslipUserType { get; init; }
        public string Environment { get; init; } = "";
        public abstract void SetCustomClaims(List<Claim> tokenClaims, TokenEncryptionSettings settings);
    }
}