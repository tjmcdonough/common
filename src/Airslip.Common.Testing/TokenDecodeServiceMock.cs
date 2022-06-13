using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Functions.Implementations;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Enums;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;

namespace Airslip.Common.Testing
{
    public static class TokenDecodeServiceMock
    {
        public static async Task<ITokenDecodeService<ApiKeyToken>> SetUp(AirslipUserType userType, ServiceCollection? additionalServices = null)
        {
            ServiceCollection serviceCollection = new();

            serviceCollection
                .AddScoped<ITokenDecodeService<ApiKeyToken>, TokenDecodeService<ApiKeyToken>>();

            if (additionalServices != null)
                serviceCollection.Add(additionalServices);

            string token = ApiKeyMock.GenerateApiKeyToken("10.0.0.0", "api-key", "entity-id", userType);

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            Mock<FunctionContext> context = new();
            context.SetupProperty(c => c.InstanceServices, serviceProvider);

            Mock<HttpRequestData> mockRequestData = new(context.Object);
            HttpHeadersCollection headerCollection = new()
            {
                {
                    AirslipSchemeOptions.ApiKeyHeaderField, token
                }
            };
            mockRequestData.Setup(data => data.Headers).Returns(headerCollection);
            Mock<IOptions<TokenEncryptionSettings>> encryptionSettings = Helpers.GenerateEncryptionSettings();
            IFunctionContextAccessor functionContextAccessor = new FunctionContextAccessor();
            IHttpContentLocator httpHeaderLocator = new FunctionContextHeaderLocator(functionContextAccessor);
            IClaimsPrincipalLocator claimsPrincipalLocator = new FunctionContextPrincipalLocator(functionContextAccessor);
            ITokenDecodeService<ApiKeyToken> tokenDecodeService = new TokenDecodeService<ApiKeyToken>(httpHeaderLocator, 
                claimsPrincipalLocator, encryptionSettings.Object);
            ITokenValidator<ApiKeyToken> tokenValidator = new TokenValidator<ApiKeyToken>(tokenDecodeService);

            ApiKeyRequestDataHandler apiKeyRequestDataHandler = new(tokenValidator, functionContextAccessor);
            await apiKeyRequestDataHandler.Handle(mockRequestData.Object);

            return tokenDecodeService;
        }
    }
}