using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Types.Enums;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public class GenerateApiKeyToken : IGenerateToken
    {
        public GenerateApiKeyToken(string entityId, string apiKey, AirslipUserType airslipUserType, string userId)
        {
            EntityId = entityId;
            ApiKey = apiKey;
            AirslipUserType = airslipUserType;
            UserId = userId;
        }

        public string UserId { get; init; }
        public string EntityId { get; init; }
        public string ApiKey { get; init; }
        public AirslipUserType AirslipUserType { get; init; }
        public string? UserRole { get; init; }
        public string[]? ApplicationRoles { get; init; }
        
        public List<Claim> GetCustomClaims(TokenEncryptionSettings settings)
        {
            List<Claim> claims = new()
            {
                new Claim(AirslipClaimTypes.API_KEY_SHORT, ApiKey),
                new Claim(AirslipClaimTypes.USER_ID_SHORT, UserId),
                new Claim(AirslipClaimTypes.USER_ROLE_SHORT, UserRole ?? string.Empty),
                new Claim(AirslipClaimTypes.APPLCATION_ROLES_SHORT, string
                    .Join(";", ApplicationRoles ?? Array.Empty<string>()))
            };

            return claims;
        }
    }
}