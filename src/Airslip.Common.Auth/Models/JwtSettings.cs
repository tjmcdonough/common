using JetBrains.Annotations;

namespace Airslip.Common.Auth.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public record JwtSettings
    {
        public string Key { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpiresTime { get; init; }
        public bool ValidateLifetime { get; init; } = true;
    }
}