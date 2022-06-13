using Airslip.Common.Auth.Implementations;
using FluentAssertions;
using System;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class JwtBearerTokenTests
    {
        [Fact]
        public void Can_generate_new_refresh_token()
        {
            string refreshToken = JwtBearerToken.GenerateRefreshToken();

            refreshToken.Should().NotBeNull();

            byte[] bytes = Convert.FromBase64String(refreshToken);
            bytes.Length.Should().Be(32);
        }
    }
}