using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Functions.Implementations;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Functions.Tests.Helpers;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Enums;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Auth.Functions.Tests
{
    public class FunctionContextAccessorTests
    {
        [Fact]
        public void Can_store_principal_in_accessor()
        {
            const string ipAddress = "10.0.0.0";
            const string apiKey = "MyNewApiKey";
            const string entityId = "MyEntityId";
            const AirslipUserType airslipUserType = AirslipUserType.Merchant;

            string newToken = HelperFunctions.GenerateApiKeyToken(ipAddress, apiKey,
                entityId, airslipUserType);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var context = new Mock<FunctionContext>();
            context.SetupProperty(c => c.InstanceServices, serviceProvider);

            // Mock the request data...
            Mock<HttpRequestData> mockRequestData = new(context.Object);
            HttpHeadersCollection headerCollection = new();
            headerCollection.Add(AirslipSchemeOptions.ApiKeyHeaderField, newToken);
            mockRequestData.Setup(data => data.Headers).Returns(headerCollection);
            mockRequestData.Setup(data => data.Url).Returns(new Uri("https://www.google.com"));
            TokenEncryptionSettings encryptionSettings = new()
            {
                UseEncryption = true,
                Passphrase = "Hello"
            };
            
            // Create the validator
            IFunctionContextAccessor functionContextAccessor = new FunctionContextAccessor();
            IHttpContentLocator httpHeaderLocator = new FunctionContextHeaderLocator(functionContextAccessor);
            IClaimsPrincipalLocator claimsPrincipalLocator =
                new FunctionContextPrincipalLocator(functionContextAccessor);
            ITokenDecodeService<ApiKeyToken> tokenDecodeService =
                new TokenDecodeService<ApiKeyToken>(httpHeaderLocator, claimsPrincipalLocator, Options.Create(encryptionSettings));
            ITokenValidator<ApiKeyToken> tokenValidator = new TokenValidator<ApiKeyToken>(tokenDecodeService);

            // Create the handler
            IApiKeyRequestDataHandler handler = new ApiKeyRequestDataHandler(tokenValidator, functionContextAccessor);

            // Now we can validate...
            Task<KeyAuthenticationResult> valid = handler.Handle(mockRequestData.Object);

            valid.Result.AuthResult.Should().Be(AuthResult.Success);

            // Finally test we can get the current token...
            ApiKeyToken currentToken = tokenDecodeService.GetCurrentToken();

            currentToken.Should().NotBeNull();

            currentToken.IpAddress.Should().Be(ipAddress);
            currentToken.ApiKey.Should().Be(apiKey);
            currentToken.EntityId.Should().Be(entityId);
            currentToken.Environment.Should().Be(AirslipSchemeOptions.ThisEnvironment);
            currentToken.AirslipUserType.Should().Be(airslipUserType);
        }

        [Fact]
        public void Can_get_key_for_token_user_service()
        {
            const string ipAddress = "10.0.0.0";
            const string apiKey = "MyNewApiKey";
            const string entityId = "MyEntityId";

            string newToken = HelperFunctions.GenerateApiKeyToken(ipAddress, apiKey, entityId);

            ServiceCollection serviceCollection = new();
            serviceCollection.AddScoped<ITokenDecodeService<ApiKeyToken>, TokenDecodeService<ApiKeyToken>>();
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            Mock<FunctionContext> context = new();
            context.SetupProperty(c => c.InstanceServices, serviceProvider);

            // Mock the request data...
            Mock<HttpRequestData> mockRequestData = new(context.Object);
            HttpHeadersCollection headerCollection = new()
            {
                {
                    AirslipSchemeOptions.ApiKeyHeaderField, newToken
                }
            };
            mockRequestData.Setup(data => data.Headers).Returns(headerCollection);
            mockRequestData.Setup(data => data.Url).Returns(new Uri("https://www.google.com"));
            
            TokenEncryptionSettings encryptionSettings = new()
            {
                UseEncryption = true,
                Passphrase = "Hello"
            };
            
            // Create the validator
            IFunctionContextAccessor functionContextAccessor = new FunctionContextAccessor();
            IHttpContentLocator httpHeaderLocator = new FunctionContextHeaderLocator(functionContextAccessor);
            IClaimsPrincipalLocator claimsPrincipalLocator = new FunctionContextPrincipalLocator(functionContextAccessor);
            ITokenDecodeService<ApiKeyToken> tokenDecodeService = 
                new TokenDecodeService<ApiKeyToken>(httpHeaderLocator, claimsPrincipalLocator, Options.Create(encryptionSettings));
            ITokenValidator<ApiKeyToken> tokenValidator = new TokenValidator<ApiKeyToken>(tokenDecodeService);

            ApiKeyRequestDataHandler apiKeyRequestDataHandler = new(tokenValidator, functionContextAccessor);
            apiKeyRequestDataHandler.Handle(mockRequestData.Object);

            // Create the handler
            ApiKeyTokenUserService apiKeyTokenUserService = new(tokenDecodeService);

            // Now we can validate...
            apiKeyTokenUserService.AirslipUserType.Should().Be(AirslipUserType.Merchant);
            apiKeyTokenUserService.EntityId.Should().Be("MyEntityId");
        }
    }
}