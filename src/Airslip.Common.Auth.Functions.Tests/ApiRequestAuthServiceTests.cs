using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Functions.Data;
using Airslip.Common.Auth.Functions.Extensions;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Functions.Tests.Helpers;
using Airslip.Common.Auth.Models;
using Airslip.Common.Testing;
using Airslip.Common.Types.Enums;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Auth.Functions.Tests;

public class ApiRequestAuthServiceTests
{
    [Theory]
    [InlineData("MyFunction", "MyFunction", "MyEntity", "MyEntity", AirslipUserType.Merchant, AirslipUserType.Merchant,
        "api_keys_validate_as_expected.json", true)]
    [InlineData("MyFunction", "NotMyFunction", "MyEntity", "MyEntity", AirslipUserType.Merchant, AirslipUserType.Merchant,
        "api_keys_validate_as_expected.json", false)]
    [InlineData("MyFunction", "MyFunction", "MyEntity", "NotMyEntity", AirslipUserType.Merchant, AirslipUserType.Merchant,
        "api_keys_validate_as_expected.json", false)]
    [InlineData("MyFunction", "MyFunction", "MyEntity", "MyEntity", AirslipUserType.Merchant, AirslipUserType.InternalApi,
        "api_keys_validate_as_expected.json", false)]
    [InlineData("MyFunction", "MyFunction", null, "MyEntity", AirslipUserType.Merchant, AirslipUserType.Merchant,
        "api_keys_validate_as_expected.json", true)]
    [InlineData("MyFunction", "MyFunction", null, "MyEntity", null, AirslipUserType.InternalApi,
        "api_keys_validate_as_expected.json", true)]
    [InlineData("MyFunction", "MyFunction", "MyEntity", "MyEntity", null, AirslipUserType.InternalApi,
        "api_keys_validate_as_expected.json", true)]
    [InlineData(null, "MyFunction", null, "MyFileEntity", null, AirslipUserType.InternalApi,
        "api_keys_validate_as_expected_from_file.json", true)]
    [InlineData(null, "MyFunction", null, "NotMyFileEntity", null, AirslipUserType.InternalApi,
        "api_keys_validate_as_expected_from_file.json", false)]
    public async Task Api_keys_validate_as_expected(
        string? functionName, string functionNameToTest, 
        string? entityId, string entityIdToTest, 
        AirslipUserType? airslipUserType, AirslipUserType airslipUserTypeToTest, 
        string settingsFile, bool accessExpected)
    {
        const string ipAddress = "10.0.0.0";
        const string apiKey = "MyNewApiKey";

        IServiceProvider provider = _generateProvider(settingsFile, functionName, airslipUserType, entityId);

        string newToken = HelperFunctions.GenerateApiKeyToken(ipAddress, apiKey,
            entityIdToTest, airslipUserTypeToTest);

        Mock<FunctionContext> context = new();
        context.SetupProperty(c => c.InstanceServices, provider);
        Mock<HttpRequestData> mockRequestData = new(context.Object);
        HttpHeadersCollection headerCollection = new() {{AirslipSchemeOptions.ApiKeyHeaderField, newToken}};
        mockRequestData.Setup(data => data.Headers).Returns(headerCollection);
        mockRequestData.Setup(data => data.Url).Returns(new Uri("https://www.google.com"));
        
        IApiRequestAuthService authRequestService = provider.GetService<IApiRequestAuthService>() 
            ?? throw new NotImplementedException();

        KeyAuthenticationResult authResult = await authRequestService
            .Handle(functionNameToTest, mockRequestData.Object);

        authResult.AuthResult.Should().Be(accessExpected ? AuthResult.Success : AuthResult.Fail);
    }

    private static IServiceProvider _generateProvider(string settingsFile, string? functionName,
        AirslipUserType? airslipUserType, string? entity)
    {
        IConfiguration config = OptionsMock.InitialiseConfiguration("Airslip.Common.Auth.Functions.Tests", 
            "Settings", settingsFile) ?? throw new NotImplementedException();

        IServiceCollection services = new ServiceCollection();
        ApiAccessOptions opts = services
            .AddAirslipFunctionAuth(config);

        if (functionName != null)
            opts.AddNamedAccessRights(functionName, airslipUserType, entity);
        
        return services.BuildServiceProvider();
    }
}