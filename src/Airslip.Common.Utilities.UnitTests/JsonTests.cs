using Airslip.Common.Types;
using Airslip.Common.Utilities.UnitTests.Base;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Airslip.Common.Utilities.UnitTests
{
    public class JsonTests
    {
        [Fact]
        public void Can_deserialize_using_camel_case_settings()
        {
            BanksResponse banks = Json.Deserialize<BanksResponse>(Factory.JsonBanksResponse);
            banks.Should().NotBeNull();
            banks.Banks.Count.Should().Be(4);
            banks.Banks.First().Name.Should().Be("ABN AMRO");
            banks.Banks.First().LogoName.Should().Be("ABN_AMRO.svg");
            banks.Banks.First().LogoUrl.Should()
                .Be("https://airslipdevstorageacct.blob.core.windows.net/airslip/banks/ABN_AMRO.svg");
            banks.Banks.First().IconName.Should().Be("ABN_AMRO_icon.svg");
            banks.Banks.First().IconUrl.Should()
                .Be("https://airslipdevstorageacct.blob.core.windows.net/airslip/banks/ABN_AMRO_icon.svg");
            banks.Banks.First().BankTypes.Count.Should().Be(1);
            banks.Banks.First().BankTypes.First().Id.Should().Be("abn-amro-nl");
            banks.Banks.First().BankTypes.First().Name.Should().Be("Abn Amro NL");
        }

        [Fact]
        public void Can_deserialize_using_default_snake_case_settings()
        {
            BanksResponse banks = Json.Deserialize<BanksResponse>(Factory.JsonBanksResponse);
            string jsonBanks = Json.Serialize(banks, Casing.SNAKE_CASE);

            jsonBanks.Should().NotBeNull();
            jsonBanks.Should().ContainAll("name","logo_name", "logo_url", "icon_name", "bank_types", "id", "name");
        }
        
        [Fact]
        public void Can_serialize_with_camel_casing_with_no_parameter_set()
        {
            BanksResponse banks = Json.Deserialize<BanksResponse>(Factory.JsonBanksResponse);
            string jsonBanks = Json.Serialize(banks);

            jsonBanks.Should().NotBeNull();
            jsonBanks.Should().ContainAll("name","logoName", "logoUrl", "iconName", "bankTypes", "id", "name");
        }
    }
}