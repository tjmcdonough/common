using Airslip.Common.Security.Implementations;
using Airslip.Common.Utilities.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Airslip.Common.Security.Tests
{
    public class HmacCipherTests
    {
        private const string SecretKey = "SECRET_KEY";
        
        [Fact]
        public void Can_decipher_a_query_string_and_match_to_a_hmac()
        {
            Dictionary<string, string> queryStrings = new()
            {
                {"shop", "mystore.airslip.com"},
                {"signature", "f477a85f3ed6027735589159f9da74da"},
                {"timestamp", "1459779785"},
            };

            string hmac = "A17AC303A0E3957183F9BF58B20203BFB38AFA890FECE30BD35BECD748A10665";
            bool isValid = HmacCipher.Validate(queryStrings, hmac, SecretKey);
            isValid.Should().BeTrue();
        }
        
        [Fact]
        public void Can_decipher_a_query_string_and_match_to_a_hmac_from_shopify_development_store()
        {
            Dictionary<string, string> queryStrings = new()
            {
                {"shop", "airslip-development.myshopify.com"},
                {"timestamp", "1639563584"}
            };

            string hmac = "848d17f2f2ccc0dd96db1be9314901f4948637cbdf2db8a56b8edd17248904d8";
            bool isValid = HmacCipher.Validate(queryStrings, hmac, "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");
            isValid.Should().BeTrue();
        }
        
        [Fact]
        public void Can_decipher_an_unordered_query_string_and_match_to_a_hmac()
        {
            Dictionary<string, string> queryStrings = new()
            {
                {"timestamp", "1459779785"},
                {"shop", "mystore.airslip.com"},
                {"signature", "f477a85f3ed6027735589159f9da74da"},
            };

            string hmac = "A17AC303A0E3957183F9BF58B20203BFB38AFA890FECE30BD35BECD748A10665";
            bool isValid = HmacCipher.Validate(queryStrings, hmac, SecretKey);
            isValid.Should().BeTrue();
        }

        [Fact]
        public void Can_remove_hmac_value_and_verify()
        {
            string queryString =
                "code=d5a16abfbe15965ddd272a37cdce8f68&hmac=d3364df5bb6541ad06e7ae51cf522f5567ce67b931f84ccab31711bea0c807d8&host=YWlyc2xpcC1kZXZlbG9wbWVudC5teXNob3BpZnkuY29tL2FkbWlu&shop=airslip-development.myshopify.com&timestamp=1639585552&state=id";

            List<KeyValuePair<string, string>> queryStrings = queryString.GetQueryParams().ToList();

            KeyValuePair<string, string> hmacKeyValuePair = queryStrings.Get("hmac");

            queryStrings.Remove(hmacKeyValuePair);
        
            bool isValid = HmacCipher.Validate(queryStrings, hmacKeyValuePair.Value, "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");
            isValid.Should().BeTrue();
        }
    }
}