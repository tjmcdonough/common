using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.UnitTests.Helpers;
using FluentAssertions;
using System;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class RemoteIpAddressServiceTests
    {
        [Fact]
        public void Can_get_ip_from_complex_forwarder()
        {
            IRemoteIpAddressService service = HelperFunctions
                .GenerateRemoteIpAddressService(withForwarder: "203.0.113.195, 70.41.3.18, 150.172.238.178");

            string userIpAddress = service.GetRequestIP();

            userIpAddress.Should().NotBeNull();
            userIpAddress.Should().Be("203.0.113.195");
        }
        
        [Fact]
        public void Can_get_ip_from_remote_addr()
        {
            IRemoteIpAddressService service = HelperFunctions
                .GenerateRemoteIpAddressService(withRemoteAddr: "203.0.113.195");

            string userIpAddress = service.GetRequestIP();

            userIpAddress.Should().NotBeNull();
            userIpAddress.Should().Be("203.0.113.195");
        }
        
        [Fact]
        public void Throws_exception_when_no_ip_address()
        {
            IRemoteIpAddressService service = HelperFunctions
                .GenerateRemoteIpAddressService();

            service.Invoking(o => o.GetRequestIP())
                .Should().Throw<ArgumentException>();
        }
    }
}