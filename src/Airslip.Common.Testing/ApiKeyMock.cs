using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace Airslip.Common.Testing
{
    public static class ApiKeyMock
    {
        public static string GenerateApiKeyToken(string withIpAddress, 
            string apiKey = "SomeApiKey", 
            string entityId = "SomeEntityId", 
            AirslipUserType airslipUserType = AirslipUserType.Merchant)
        {
            ITokenGenerationService<GenerateApiKeyToken> service = CreateTokenGenerationService<GenerateApiKeyToken>(withIpAddress, "");
            
            GenerateApiKeyToken apiTokenKey = new(
                entityId,
                apiKey,
                airslipUserType);
            
            return service.GenerateNewToken(apiTokenKey).TokenValue;
        }
        
        public static ITokenGenerationService<TTokenType> CreateTokenGenerationService<TTokenType>(
            string withIpAddress, 
            string withToken, 
            string withKey = "WowThisIsSuchASecureKeyICantBelieveIt",
            ClaimsPrincipal? withClaimsPrincipal = null,
            string withUserAgent = CommonAgents.UA_WINDOWS_10_EDGE) where TTokenType : IGenerateToken
        {
            Mock<IOptions<JwtSettings>> options = GenerateOptionsWithKey(withKey);
            Mock<IRemoteIpAddressService> ipService = GenerateMockRemoteIpAddressService(withIpAddress);
            IUserAgentService userAgentService = GenerateUserAgentService(withUserAgent);
            Mock<IOptions<TokenEncryptionSettings>> encryptionSettings = Helpers.GenerateEncryptionSettings();

            TokenGenerationService<TTokenType> service = 
                new(options.Object, ipService.Object, userAgentService, encryptionSettings.Object);

            return service;
        }
        
        public static Mock<IOptions<JwtSettings>> GenerateOptionsWithKey(string key)
        {
            JwtSettings settings = new()
            {
                Audience = "Some Audience",
                Issuer = "Some Issuer",
                Key = key,
                ExpiresTime = 3600,
                ValidateLifetime = false
            };
            Mock<IOptions<JwtSettings>> options = new();
            options.Setup(_ => _.Value).Returns(settings);

            return options;
        }
        
        public static Mock<IRemoteIpAddressService> GenerateMockRemoteIpAddressService(string ipAddress)
        {
            Mock<IRemoteIpAddressService> remoteIpAddressService = new();
            remoteIpAddressService.Setup(_ => _.GetRequestIP()).Returns(ipAddress);

            return remoteIpAddressService;
        }
        
        public static IUserAgentService GenerateUserAgentService(string withUserAgent = CommonAgents.UA_WINDOWS_10_EDGE)
        {
            Mock<IHttpContextAccessor> accessor = HttpContextAccessorMock.GenerateContext("", TokenTypeMock.BearerToken, 
                withUserAgent);
            UserAgentService userAgentService = new(accessor.Object);

            return userAgentService;
        }
    }
}