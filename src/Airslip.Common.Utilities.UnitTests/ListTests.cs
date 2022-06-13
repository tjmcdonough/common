using Airslip.Common.Utilities.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Airslip.Common.Utilities.UnitTests
{
    public class ListTests
    {
        [Fact]
        public void Can_remove_first_value_from_list()
        {
            List<string> list = new()
            {
                "0","1","2"
            };

            list.RemoveFirst();
            
            List<string> expectedList = new()
            {
                "1","2"
            };

            list.Should().BeEquivalentTo(expectedList);
        }
    }
}