using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Airslip.Common.Auth.Implementations
{
    public class TokenGenerationService<TGenerateTokenType> : ITokenGenerationService<TGenerateTokenType> 
        where TGenerateTokenType : IGenerateToken
    {
        private readonly IRemoteIpAddressService _remoteIpAddressService;
        private readonly IUserAgentService _userAgentService;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly TokenEncryptionSettings _settings;

        public TokenGenerationService(IOptions<JwtSettings> jwtSettings, 
            IRemoteIpAddressService remoteIpAddressService,
            IUserAgentService userAgentService,
            IOptions<TokenEncryptionSettings> options)
        {
            _remoteIpAddressService = remoteIpAddressService;
            _userAgentService = userAgentService;
            _jwtSettings = jwtSettings.Value;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _settings = options.Value;
        }

        public NewToken GenerateNewToken(ICollection<Claim> claims)
        {
            SigningCredentials signingCredentials = getSigningCredentials();

            DateTime? expiryDate =
                _jwtSettings.ExpiresTime > 0 ? DateTime.Now.AddSeconds(_jwtSettings.ExpiresTime) : null;
            
            JwtSecurityToken token = new(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: expiryDate,
                signingCredentials: signingCredentials);

            return new NewToken(_jwtSecurityTokenHandler.WriteToken(token), expiryDate);
        }

        public NewToken GenerateNewToken(TGenerateTokenType token)
        {
            List<Claim> claims = new()
            {
                new Claim(AirslipClaimTypes.CORRELATION, CommonFunctions.GetId().Encrypt(_settings)),
                new Claim(AirslipClaimTypes.AIRSLIP_USER_TYPE, token.AirslipUserType.ToString().Encrypt(_settings)),
                new Claim(AirslipClaimTypes.ENTITY_ID, token.EntityId.Encrypt(_settings)),
                new Claim(AirslipClaimTypes.ENVIRONMENT, AirslipSchemeOptions.ThisEnvironment.Encrypt(_settings)),
                new Claim(AirslipClaimTypes.IP_ADDRESS, (_remoteIpAddressService.GetRequestIP() ?? "UNKNOWN").Encrypt(_settings)),
                new Claim(AirslipClaimTypes.USER_AGENT, (_userAgentService.GetRequestUserAgent() ?? "UNKNOWN").Encrypt(_settings))
            };

            claims.AddRange(token.GetCustomClaims(_settings));

            return GenerateNewToken(claims);
        }
        
        private SigningCredentials getSigningCredentials()
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Key))
                throw new ArgumentNullException(nameof(_jwtSettings.Key), "private key must be set");

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            if (key.KeySize < 256)
                throw new ArgumentException(
                    "the key must be at least 256 in length -> 32 characters in length at least",
                    nameof(_jwtSettings.Key));

            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
    }
}