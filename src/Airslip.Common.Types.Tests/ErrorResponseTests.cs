using Airslip.Common.Types.Failures;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class ErrorResponseTests
    {
        [Fact]
        public void Can_deserialize_error()
        {
            string errorContent =
                "{\"errors\":[{\"errorCode\":\"8\",\"message\":\"Store https://airslip-development.myshopify.com already exists!\",\"metadata\":{ \"test\": \"123\" },\"links\":[]}]}";

            ErrorResponses errorResponses = JsonConvert.DeserializeObject<ErrorResponses>(errorContent);

            ErrorResponse firstError = errorResponses.Errors.First();

            firstError.ErrorCode.Should().Be("8");
            firstError.Message.Should()
                .Be("Store https://airslip-development.myshopify.com already exists!");

            errorResponses.Errors.First().Metadata.Should()
                .BeEquivalentTo(new Dictionary<string, object> {{"test", "123"}});
        }
    }
}