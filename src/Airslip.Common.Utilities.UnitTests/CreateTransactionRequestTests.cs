using Airslip.Common.Types;
using FluentAssertions;
using Xunit;

namespace Airslip.Common.Utilities.UnitTests
{
    public class CreateTransactionRequestTests
    {
        [Fact]
        public void Can_deserialize_snake_case_request_with_camelcase_named_properties()
        {
            JsonRequest startingRequest = new()
            {
                CreatedAt = "2021-11-11 09:55:00",
                FieldWithNumbers2 = "test"
            };
            string serialisedJson =  Json.Serialize(startingRequest, Casing.SNAKE_CASE);
            JsonRequest deserializedRequest = Json.Deserialize<JsonRequest>(serialisedJson, Casing.SNAKE_CASE);
            deserializedRequest.CreatedAt.Should().Be("2021-11-11 09:55:00");
            deserializedRequest.FieldWithNumbers2.Should().Be("test");
            deserializedRequest.NullableField.Should().BeNull();
        }
    }

    public class JsonRequest
    {
        public string CreatedAt { get; set; } = string.Empty;
        public string? FieldWithNumbers2 { get; set; }
        public string? NullableField { get; set; }
    }
}