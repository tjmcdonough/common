using System;
using System.Security.Cryptography;

namespace Airslip.Common.Auth.Implementations
{
    public static class JwtBearerToken
    {
        public static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using RandomNumberGenerator numberGenerator = RandomNumberGenerator.Create();
            numberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
