using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.UnitTests.Helpers;
using FluentAssertions;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class ApiKeyValidatorTests
    {
        [Fact]
        public async Task Can_get_claims_principal_from_api_key_token()
        {
            string newToken = HelperFunctions.GenerateApiKeyToken("10.0.0.0");

            ITokenValidator<ApiKeyToken> apiKeyValidator = HelperFunctions.GenerateCryptoValidator<ApiKeyToken>(TokenType.ApiKey);

            ClaimsPrincipal claimsPrincipal = await apiKeyValidator.GetClaimsPrincipalFromToken(newToken, 
                AirslipSchemeOptions.ApiKeyScheme,
                AirslipSchemeOptions.ThisEnvironment);

            claimsPrincipal.Should().NotBeNull();
            claimsPrincipal?.Claims.Count().Should().BeGreaterThan(0);
        }
        
        [Fact]
        public async Task Fails_with_invalid_api_key_token()
        {
            ITokenValidator<ApiKeyToken> apiKeyValidator = HelperFunctions
                .GenerateCryptoValidator<ApiKeyToken>(TokenType.ApiKey);

            await apiKeyValidator
                .Invoking(y => y.
                    GetClaimsPrincipalFromToken("I am an invalid token", AirslipSchemeOptions.ApiKeyScheme,
                        AirslipSchemeOptions.ThisEnvironment))
                .Should()
                .ThrowAsync<ArgumentException>()
                .WithParameterName("tokenValue");
        }
    }
}