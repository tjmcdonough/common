using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Airslip.Common.Auth.Implementations
{
    public class CryptoTokenDecodeService<TTokenType> : ITokenDecodeService<TTokenType> 
        where TTokenType : IDecodeToken, new()
    {
        private readonly IHttpContentLocator _httpContentLocator;
        private readonly IClaimsPrincipalLocator _claimsPrincipalLocator;
        private readonly TokenEncryptionSettings _settings;

        public CryptoTokenDecodeService(IHttpContentLocator httpContentLocator, 
            IClaimsPrincipalLocator claimsPrincipalLocator,
            IOptions<TokenEncryptionSettings> options)
        {
            _httpContentLocator = httpContentLocator;
            _claimsPrincipalLocator = claimsPrincipalLocator;
            _settings = options.Value with
            {
                UseEncryption = true
            };
        }

        public Tuple<TTokenType, ICollection<Claim>> DecodeToken(string tokenValue)
        {
            try
            {
                string decryptedString = tokenValue.Decrypt(_settings);
                string[] values = decryptedString.Split("~");
                IEnumerable<string[]> splitValues = values
                    .Select(o => o.Split("@"))
                    .ToList();
                
                List<Claim> claims = splitValues
                    .Select(o => new Claim(o[0].MapToLong(), o[1]))
                    .ToList();

                claims.AddRange(splitValues
                    .Select(o => new Claim(o[0], o[1]))
                    .Where(o => !claims.Any(p => p.Type.Equals(o.Type)))
                    .ToList());
                
                return new Tuple<TTokenType, ICollection<Claim>>(GenerateTokenFromClaims(claims, true),
                    claims);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Token is not in expected format", nameof(tokenValue));
            }
        }

        public Tuple<TTokenType, ICollection<Claim>> DecodeTokenFromHeader(string headerValue)
        {
            return DecodeTokenFromHeader(headerValue, string.Empty);
        }

        public Tuple<TTokenType, ICollection<Claim>> DecodeTokenFromHeader(string headerValue, string withScheme)
        {
            string rawToken = _httpContentLocator.GetHeaderValue(headerValue) ?? 
                              throw new ArgumentException("Header not found", nameof(headerValue));

            switch (withScheme)
            {
                case AirslipSchemeOptions.JwtBearerScheme:
                    // This should be predictable - we take the bearer off then trim the token
                    rawToken = rawToken.Replace(AirslipSchemeOptions.JwtBearerScheme, "").TrimStart();
                    break;
            }
            
            return DecodeToken(rawToken);
        }

        public TTokenType GetCurrentToken()
        {
            ClaimsPrincipal? claimsPrincipal = _claimsPrincipalLocator.GetCurrentPrincipal();

            List<Claim> claims = claimsPrincipal?.Claims.ToList() ?? new List<Claim>();
            
            return GenerateTokenFromClaims(claims, claimsPrincipal?.Identity?.IsAuthenticated ?? false);
        }

        public TTokenType GenerateTokenFromClaims(ICollection<Claim> tokenClaims, bool? isAuthenticated)
        {
            if (!Enum.TryParse(tokenClaims.GetValue(AirslipClaimTypes.AIRSLIP_USER_TYPE_SHORT), 
                out AirslipUserType airslipUserType))
            {
                airslipUserType = AirslipUserType.Merchant;
            }

            TTokenType result = new()
            {
                CorrelationId = CommonFunctions.GetId(),
                AirslipUserType = airslipUserType,
                IsAuthenticated = isAuthenticated,
                EntityId = tokenClaims.GetValue(AirslipClaimTypes.ENTITY_ID_SHORT),
                Environment = tokenClaims.GetValue(AirslipClaimTypes.ENVIRONMENT_SHORT)
            };

            result
                .SetCustomClaims(tokenClaims.ToList(), _settings);

            return result;
        }
    }
}