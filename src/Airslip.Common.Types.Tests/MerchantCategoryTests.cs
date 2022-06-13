using Airslip.Common.Types.Enums;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Airslip.Common.Types.Tests;

public class MerchantCategoryTests
{
    [Fact]
    public void Can_load_value_for_iso_18245_category_codes()
    {
        string merchantCategoryValue = Iso18245MerchantCategoryCodes.LoadValue("1731");
        merchantCategoryValue.Should().Be("Electrical contractors");
    }
    
    [Fact]
    public void Can_get_all_codes_for_iso_18245_category_codes()
    {
        Dictionary<string, string> merchantCategories = Iso18245MerchantCategoryCodes.Get();
        merchantCategories.Count.Should().Be(367);
    }
}