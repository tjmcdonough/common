using Airslip.Common.Utilities.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Airslip.Common.Utilities.UnitTests
{
    public class DictionaryTests
    {
        [Fact]
        public void Can_get_object_value_from_dictionary()
        {
            Dictionary<string, object> dictionary = new() { { "colour", "Sunblaze-Puma Black" } };

            object? value = dictionary.GetValue("colour");

            value!.ToString().Should().Be("Sunblaze-Puma Black");
        }
        [Fact]
        public void Can_get_value_from_dictionary()
        {
            Dictionary<string, string> dictionary = new() { { "colour", "Sunblaze-Puma Black" } };

            string? value = dictionary.GetValue("colour");

            value!.Should().Be("Sunblaze-Puma Black");
        }
    }
}