using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public record KeyAuthenticationResult
    {
        public AuthResult AuthResult { get; init; }
        public string? Message { get; init; }
        public ClaimsPrincipal? Principal { get; set; }

        public static KeyAuthenticationResult Fail(string message)
        {
            return new KeyAuthenticationResult
            {
                AuthResult = AuthResult.Fail,
                Message = message
            };
        }
        
        public static KeyAuthenticationResult NoResult()
        {
            return new KeyAuthenticationResult
            {
                AuthResult = AuthResult.NoResult
            };
        }

        public static KeyAuthenticationResult Valid(ClaimsPrincipal keyPrincipal)
        {
            return new KeyAuthenticationResult
            {
                AuthResult = AuthResult.Success,
                Principal = keyPrincipal
            };
        }
    }

    public enum AuthResult
    {
        Success,
        Fail,
        NoResult
    }
}