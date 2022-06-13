using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Airslip.Common.Auth.Implementations
{
    public class CryptoTokenGenerationService<TGenerateTokenType> : ITokenGenerationService<TGenerateTokenType> 
        where TGenerateTokenType : IGenerateToken
    {
        private readonly TokenEncryptionSettings _settings;

        public CryptoTokenGenerationService(IOptions<TokenEncryptionSettings> options)
        {
            _settings = options.Value with
            {
                UseEncryption = true
            };
        }

        public NewToken GenerateNewToken(ICollection<Claim> claims)
        {
            List<string> claimList = new();

            claimList.AddRange(claims.Select(o => $"{o.Type}@{o.Value}"));

            string encryptedString = string.Join("~", claimList).Encrypt(_settings);

            return new NewToken(encryptedString, null);
        }

        public NewToken GenerateNewToken(TGenerateTokenType token)
        {
            List<Claim> claims = new()
            {
                new Claim(AirslipClaimTypes.AIRSLIP_USER_TYPE_SHORT, $"{(int)token.AirslipUserType}"),
                new Claim(AirslipClaimTypes.ENTITY_ID_SHORT, token.EntityId),
                new Claim(AirslipClaimTypes.ENVIRONMENT_SHORT, AirslipSchemeOptions.ThisEnvironment)
            };

            claims.AddRange(token.GetCustomClaims(_settings));

            return GenerateNewToken(claims);
        }
    }
}