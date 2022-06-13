using Airslip.Common.Utilities.Extensions;
using FluentAssertions;
using Xunit;

namespace Airslip.Common.Utilities.UnitTests
{
    public class CasingExtensionsTests
    {
        [Fact]
        public void Can_get_snake_case_from_camel_case()
        {
            string str = "BarclaycardCommercialPayments";

            string snakeCaseString = str.ToSnakeCase();
            
            string expectedString = "barclaycard_commercial_payments";

            snakeCaseString.Should().Be(expectedString);
        }
        
        [Fact]
        public void Can_get_snake_case_from_spaced_camel_case()
        {
            string str = "Barclaycard Commercial Payments";
            string expectedString = "barclaycard_commercial_payments";

            string snakeCaseString = str.ToSnakeCase();
            
            snakeCaseString.Should().Be(expectedString);
        }
        
        [Theory]
        [InlineData("ICanGetAString")]
        [InlineData("ICan_GetAString")]
        [InlineData("i-can-get-a-string")]
        [InlineData("i_can_get_a_string")]
        [InlineData("I Can Get A String")]
        public void Can_get_kebab_case_from_string(string value)
        {
            string expectedString = "i-can-get-a-string";

            string snakeCaseString = value.ToKebabCasing();
            
            snakeCaseString.Should().Be(expectedString);
        }
        
        [Theory]
        [InlineData("I|Can|Get|A|String")]
        [InlineData("ICan_GetAString")]
        [InlineData("i-can-get-a-string")]
        [InlineData("i_can_get_a_string")]
        [InlineData("I Can Get A String")]
        [InlineData("ICanGetAString")]
        public void Can_get_spaced_pascal_case_from_string_with_single_letter_at_start(string value)
        {
            string expectedString = "I Can Get A String";

            string spacedPascalCase = value.ToSpacedPascalCase();
            
            spacedPascalCase.Should().Be(expectedString);
        }
        
        [Theory]
        [InlineData("Lovely|A|String")]
        [InlineData("Lovely_AString")]
        [InlineData("lovely-a-string")]
        [InlineData("lovely_a_string")]
        [InlineData("Lovely A String")]
        [InlineData("LovelyAString")]
        public void Can_get_spaced_pascal_case_from_string_with_single_letter_in_middle(string value)
        {
            string expectedString = "Lovely A String";

            string spacedPascalCase = value.ToSpacedPascalCase();
            
            spacedPascalCase.Should().Be(expectedString);
        }
        
        [Theory]
        [InlineData("Lovely|A|String")]
        [InlineData("Lovely_AString")]
        [InlineData("lovely-a-string")]
        [InlineData("lovely_a_string")]
        [InlineData("Lovely A String")]
        [InlineData("LovelyAString")]
        public void Can_get_pascal_case_from_string_with_single_letter_in_middle(string value)
        {
            string expectedString = "LovelyAString";

            string spacedPascalCase = value.ToPascalCase();
            
            spacedPascalCase.Should().Be(expectedString);
        }
        
        [Theory]
        [InlineData("I|Can|Get|A|String")]
        [InlineData("ICan_GetAString")]
        [InlineData("i-can-get-a-string")]
        [InlineData("i_can_get_a_string")]
        [InlineData("I Can Get A String")]
        [InlineData("ICanGetAString")]
        public void Can_get_camel_case_from_string_with_single_letter_at_start(string value)
        {
            string expectedString = "iCanGetAString";

            string spacedPascalCase = value.ToCamelCase();
            
            spacedPascalCase.Should().Be(expectedString);
        }
    }
}