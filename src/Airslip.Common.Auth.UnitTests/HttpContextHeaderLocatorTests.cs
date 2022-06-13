using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class HttpHeaderLocatorTests
    {
        [Fact]
        public void Can_decode_null_httpcontext()
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            IHttpContentLocator locator = new HttpContextContentLocator(mockHttpContextAccessor.Object);

            locator.GetHeaderValue("Hello", "MyDefault").Should().Be("MyDefault");
        }
        
        [Fact]
        public void Can_decode_not_null_httpcontext()
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            DefaultHttpContext context = new();
            context.Request.Headers["Authorization"] = "Bearer SomeToken";

            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            
            IHttpContentLocator locator = new HttpContextContentLocator(mockHttpContextAccessor.Object);

            locator.GetHeaderValue("Authorization", "MyDefault").Should().Be("Bearer SomeToken");
        }
    }
}