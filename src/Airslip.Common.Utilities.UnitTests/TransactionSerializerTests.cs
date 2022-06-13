using FluentAssertions;
using Xunit;
using Airslip.Common.Utilities.Implementations;

namespace Airslip.Common.Utilities.UnitTests
{
    public class TransactionSerializerTests
    {
        [Theory]
        [InlineData("518791******1234")]
        [InlineData("**1234")]
        [InlineData("1234")]
        public void Can_get_last_card_digits_from_masked_pan_number_for_non_amex(string maskedPanNumber)
        {
            TransactionSerializer transactionSerializer = new();

            string? lastCardDigits = transactionSerializer.GetLastCardDigits(maskedPanNumber);

            lastCardDigits.Should().NotBeNull();

            lastCardDigits.Should().Be("1234");
        }

        [Theory]
        [InlineData("518791******12345")]
        [InlineData("**12345")]
        [InlineData("12345")]
        [InlineData("xxxx-xxxxxx-12345")]
        public void Can_get_last_card_digits_from_masked_pan_number_for_amex(string maskedPanNumber)
        {
            TransactionSerializer transactionSerializer = new();

            string? lastCardDigits = transactionSerializer.GetLastCardDigits(maskedPanNumber);

            lastCardDigits.Should().NotBeNull();

            lastCardDigits.Should().Be("12345");
        }

        [Fact]
        public void Empty_masked_pan_number_returns_null()
        {
            TransactionSerializer transactionSerializer = new();

            string? lastCardDigits = transactionSerializer.GetLastCardDigits(null);

            lastCardDigits.Should().BeNull();
        }
    }
}