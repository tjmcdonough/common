using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities.Extensions;
using FluentAssertions;
using System.Text;
using Xunit;

namespace Airslip.Common.Utilities.UnitTests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void Can_remove_accents_from_string()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string str = "Caffè Nero";
            string expectedString = "Caffe Nero";

            string normalisedString = str.RemoveAccents();
            
            normalisedString.Should().Be(expectedString);
        }
        
        [Theory]
        [InlineData("shopify")]
        [InlineData("Shopify")]
        public void Can_try_parse_enum_ignoring_case(string posProvider)
        {
            bool canParse = posProvider.TryParseIgnoreCase(out PosProviders provider);

            canParse.Should().BeTrue();
            Assert.True(provider == PosProviders.Shopify);
        }
        
        [Theory]
        [InlineData("shopify")]
        [InlineData("Shopify")]
        public void Can_parse_enum_ignoring_case(string posProvider)
        {
            PosProviders provider = posProvider.ParseIgnoreCase<PosProviders>();

            Assert.True(provider == PosProviders.Shopify);
        }
    }
}