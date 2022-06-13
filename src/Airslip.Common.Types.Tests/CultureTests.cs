using System.Globalization;
using FluentAssertions;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class CultureTests
    {
        [Fact]
        public void Can_get_british_currency_symbol_from_iso4217_currencyCode()
        {
            string currencySymbol = Culture.GetCurrencySymbol("GBP");
            currencySymbol.Should().Be("£");
        }
        
        [Fact]
        public void Can_get_american_currency_symbol_from_iso4217_currencyCode()
        {
            string currencySymbol = Culture.GetCurrencySymbol("USD");
            currencySymbol.Should().Be("$");
        }
        
        [Fact]
        public void Can_get_euro_currency_symbol_from_iso4217_currencyCode()
        {
            string currencySymbol = Culture.GetCurrencySymbol("EUR");
            currencySymbol.Should().Be("€");
        }
    }
}