using FluentAssertions;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class CurrencyTests
    {
        [Theory]
        [InlineData("12.99")]
        [InlineData("-12.99")]
        public void Can_convert_string_decimal_to_unit_value(string value)
        {
            long? pennies = Currency.ConvertToUnit(value);
            pennies.Should().Be(1299);
        }
        
        [Theory]
        [InlineData("12")]
        [InlineData("-12")]
        public void Can_convert_string_whole_number_to_unit_value(string value)
        {
            long? pennies = Currency.ConvertToUnit(value);
            pennies.Should().Be(1200);
        }

        [Fact]
        public void Empty_currency_returns_null()
        {
            long? pennies = Currency.ConvertToUnit("");
            pennies.Should().Be(null);
        }
        
        [Theory]
        [InlineData(12.99)]
        [InlineData(-12.99)]
        public void Can_convert_double_to_unit_value(double value)
        {
            long? pennies = Currency.ConvertToUnit(value);
            pennies.Should().Be(1299);
        }
        
        [Theory]
        [InlineData(12.99)]
        [InlineData(-12.99)]
        public void Can_convert_decimal_to_unit_value(decimal value)
        {
            long? pennies = Currency.ConvertToUnit(value);
            pennies.Should().Be(1299);
        }
        
        [Theory]
        [InlineData(12)]
        [InlineData(-12)]
        public void Can_convert_double_whole_number_to_unit_value(double value)
        {
            long? pennies = Currency.ConvertToUnit(value);
            pennies.Should().Be(1200);
        }
        
        [Theory]
        [InlineData(12)]
        [InlineData(-12)]
        public void Can_convert_decimal_whole_number_to_unit_value(decimal value)
        {
            long? pennies = Currency.ConvertToUnit(value);
            pennies.Should().Be(1200);
        }
        
        [Theory]
        [InlineData(1299)]
        [InlineData(-1299)]
        public void Can_convert_unit_value_to_decimal(long value)
        {
            decimal? currency = Currency.ConvertToTwoPlacedDecimal(value);
            currency.Should().Be((decimal)12.99);
        }
        
        [Theory]
        [InlineData(63.47826)]
        [InlineData(-63.47826)]
        public void Can_convert_five_place_decimal_from_double_to_unit_long(double value)
        {
            decimal? currency = Currency.ConvertFivePlaceDecimalToUnit(value);
            currency.Should().Be(6347);
        }
        
        [Theory]
        [InlineData("63.47826")]
        [InlineData("-63.47826")]
        public void Can_convert_five_place_decimal_from_string_to_unit_long(string value)
        {
            decimal? currency = Currency.ConvertFivePlaceDecimalToUnit(value);
            currency.Should().Be(6347);
        }
    }
}