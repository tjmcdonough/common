using Airslip.Common.Auth.Models;
using Microsoft.Extensions.Options;
using Moq;

namespace Airslip.Common.Testing
{
    public static class Helpers
    {
        public static Mock<IOptions<TokenEncryptionSettings>> GenerateEncryptionSettings()
        {
            TokenEncryptionSettings settings = new()
            { 
                UseEncryption = true,
                Passphrase = "SomePassphrase"
            };
            Mock<IOptions<TokenEncryptionSettings>> options = new();
            options.Setup(_ => _.Value).Returns(settings);

            return options;
        }
    }
}