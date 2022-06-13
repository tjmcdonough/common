using JetBrains.Annotations;

namespace Airslip.Common.Auth.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public record TokenEncryptionSettings
    {
        public bool UseEncryption { get; init; } = true;
        public string Passphrase { get; init; } = string.Empty;
    }
}