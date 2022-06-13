using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.UnitTests.Helpers;
using Airslip.Common.Types.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class ApiKeyTokenServiceTests
    {
        [Fact]
        public void Can_generate_new_token()
        {
            string newToken = HelperFunctions.GenerateApiKeyToken("10.0.0.0");

            newToken.Should().NotBeNullOrWhiteSpace();
        }
        
        [Fact]
        public void Can_generate_new_token_with_null_ip()
        {
            string newToken = HelperFunctions.GenerateApiKeyToken();

            newToken.Should().NotBeNullOrWhiteSpace();
        }
        
        [Fact]
        public void Can_decode_token()
        {
            const string apiKey = "MyNewApiKey";
            const string entityId = "MyEntityId";
            const AirslipUserType airslipUserType = AirslipUserType.Merchant;
            
            string newToken = HelperFunctions.GenerateApiKeyToken(apiKey, entityId, 
                airslipUserType);

            ITokenDecodeService<ApiKeyToken> service = HelperFunctions.
                CreateCyrptoTokenDecodeService<ApiKeyToken>(newToken, TokenType.ApiKey);
            
            Tuple<ApiKeyToken, ICollection<Claim>> decodedToken = service.DecodeToken(newToken);

            decodedToken.Should().NotBeNull();

            decodedToken.Item1.ApiKey.Should().Be(apiKey);
            decodedToken.Item1.EntityId.Should().Be(entityId);
            decodedToken.Item1.Environment.Should().Be(AirslipSchemeOptions.ThisEnvironment);
            decodedToken.Item1.AirslipUserType.Should().Be(airslipUserType);
        }
        
        [Fact]
        public async Task Can_decode_token_from_principal()
        {
            const string apiKey = "MyNewApiKey";
            const string entityId = "MyEntityId";
            const AirslipUserType airslipUserType = AirslipUserType.Merchant;
            
            string newToken = HelperFunctions.GenerateApiKeyToken(apiKey, 
                entityId, airslipUserType);

            // Prepare test data...
            ITokenValidator<ApiKeyToken> tempService = HelperFunctions.GenerateCryptoValidator<ApiKeyToken>(TokenType.ApiKey);
            ClaimsPrincipal claimsPrincipal = await tempService.GetClaimsPrincipalFromToken(newToken, 
                AirslipSchemeOptions.ApiKeyScheme,
                AirslipSchemeOptions.ThisEnvironment);

            ITokenDecodeService<ApiKeyToken> service = HelperFunctions.
                CreateCyrptoTokenDecodeService<ApiKeyToken>(newToken, TokenType.ApiKey, claimsPrincipal);
            
            ApiKeyToken currentToken = service.GetCurrentToken();

            currentToken.Should().NotBeNull();

            currentToken.ApiKey.Should().Be(apiKey);
            currentToken.EntityId.Should().Be(entityId);
            currentToken.Environment.Should().Be(AirslipSchemeOptions.ThisEnvironment);
            currentToken.AirslipUserType.Should().Be(airslipUserType);
        }
                
        [Fact]
        public void Can_generate_new_token_with_claims()
        {
            ITokenGenerationService<GenerateApiKeyToken> service = HelperFunctions
                .CreateCryptoTokenGenerationService<GenerateApiKeyToken>();

            List<Claim> claims = new()
            {
                new Claim("Name", "Value")
            };

            string newToken = service.GenerateNewToken(claims).TokenValue;
            
            newToken.Should().NotBeNullOrWhiteSpace();
        }
    }
}