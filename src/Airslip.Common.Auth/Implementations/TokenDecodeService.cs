using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Airslip.Common.Auth.Implementations
{
    public class TokenDecodeService<TTokenType> : ITokenDecodeService<TTokenType> 
        where TTokenType : IDecodeToken, new()
    {
        private readonly IHttpContentLocator _httpContentLocator;
        private readonly IClaimsPrincipalLocator _claimsPrincipalLocator;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly TokenEncryptionSettings _settings;

        public TokenDecodeService(IHttpContentLocator httpContentLocator, 
            IClaimsPrincipalLocator claimsPrincipalLocator,
            IOptions<TokenEncryptionSettings> options)
        {
            _httpContentLocator = httpContentLocator;
            _claimsPrincipalLocator = claimsPrincipalLocator;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _settings = options.Value;
        }

        public Tuple<TTokenType, ICollection<Claim>> DecodeToken(string tokenValue)
        {
            try
            {
                JwtSecurityToken token = _jwtSecurityTokenHandler.ReadJwtToken(tokenValue);

                return new Tuple<TTokenType, ICollection<Claim>>(GenerateTokenFromClaims(token.Claims.ToList(), true),
                    token.Claims.ToList());
            }
            catch (ArgumentException)
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
            string correlationId = tokenClaims.GetValue(AirslipClaimTypes.CORRELATION).Decrypt(_settings);
            if (!Enum.TryParse(tokenClaims.GetValue(AirslipClaimTypes.AIRSLIP_USER_TYPE).Decrypt(_settings), 
                out AirslipUserType airslipUserType))
            {
                airslipUserType = AirslipUserType.Merchant;
            }

            TTokenType result = new()
            {
                CorrelationId = string.IsNullOrWhiteSpace(correlationId) ? CommonFunctions.GetId() : correlationId,
                AirslipUserType = airslipUserType,
                IsAuthenticated = isAuthenticated,
                BearerToken = _httpContentLocator.GetHeaderValue("Authorization") ?? "",
                IpAddress = tokenClaims.GetValue(AirslipClaimTypes.IP_ADDRESS).Decrypt(_settings),
                EntityId = tokenClaims.GetValue(AirslipClaimTypes.ENTITY_ID).Decrypt(_settings),
                Environment = tokenClaims.GetValue(AirslipClaimTypes.ENVIRONMENT).Decrypt(_settings),
                UserAgent = tokenClaims.GetValue(AirslipClaimTypes.USER_AGENT).Decrypt(_settings) 
            };

            result
                .SetCustomClaims(tokenClaims.ToList(), _settings);

            return result;
        }
    }
}