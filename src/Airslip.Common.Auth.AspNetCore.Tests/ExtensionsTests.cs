using Airslip.Common.Auth.AspNetCore.Extensions;
using Airslip.Common.Auth.AspNetCore.Implementations;
using Airslip.Common.Auth.AspNetCore.Interfaces;
using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Airslip.Common.Auth.AspNetCore.Tests
{
    public class ExtensionsTests
    {
        [Fact]
        public void Can_add_services_to_service_collection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            serviceCollection.AddAirslipJwtAuth(configurationBuilder.Build(), AuthType.All);

            int count = serviceCollection.Count(o => o.ServiceType.FullName.Contains("Airslip"));
            count.Should().Be(17);
        }
        
        [Fact]
        public void Can_construct_required_services()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddAirslipJwtAuth(configurationBuilder.Build(), AuthType.ApiKey);

            var provider = serviceCollection.BuildServiceProvider();

            var obj1 = provider.GetService<ITokenDecodeService<ApiKeyToken>>();
            var obj2 = provider.GetService<IApiKeyRequestHandler>();

            obj1.Should().BeAssignableTo<CryptoTokenDecodeService<ApiKeyToken>>();
            obj2.Should().BeAssignableTo<ApiKeyRequestHandler>();
        }
        
        [Fact]
        public void Can_construct_required_services_api_access()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddApiAccessValidation(configurationBuilder.Build());

            var provider = serviceCollection.BuildServiceProvider();

            var obj1 = provider.GetService<IApiRequestAuthService>();

            obj1.Should().BeAssignableTo<ApiRequestAuthService>();
        }
    }
}