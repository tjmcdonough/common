using Airslip.Common.Utilities.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Xunit;

namespace Airslip.Common.Utilities.UnitTests
{
    public class WebExtensionsTests
    {
        [Fact]
        public void Can_get_query_params_of_unordered_query_string()
        {
            string queryString =
                "https://secure.airslip.com?code=d5a16abfbe15965ddd272a37cdce8f68&hmac=d3364df5bb6541ad06e7ae51cf522f5567ce67b931f84ccab31711bea0c807d8&host=YWlyc2xpcC1kZXZlbG9wbWVudC5teXNob3BpZnkuY29tL2FkbWlu&shop=airslip-development.myshopify.com&timestamp=1639585552&state=id";

            IEnumerable<KeyValuePair<string, string>> queryStringKeyValuePairs = queryString.GetQueryParams();

            List<KeyValuePair<string, string>> expectedKeyValuePairs = new()
            {
                new KeyValuePair<string, string>("code", "d5a16abfbe15965ddd272a37cdce8f68"),
                new KeyValuePair<string, string>("hmac", "d3364df5bb6541ad06e7ae51cf522f5567ce67b931f84ccab31711bea0c807d8"),
                new KeyValuePair<string, string>("host", "YWlyc2xpcC1kZXZlbG9wbWVudC5teXNob3BpZnkuY29tL2FkbWlu"),
                new KeyValuePair<string, string>("shop", "airslip-development.myshopify.com"),
                new KeyValuePair<string, string>("state", "id"),
                new KeyValuePair<string, string>("timestamp", "1639585552")
            };

            queryStringKeyValuePairs.Should().BeEquivalentTo(expectedKeyValuePairs);
        }

        [Fact]
        public void Can_validate_string_with_base_64_encoded_string()
        {
            string queryString =
                "https://secure.vendhq.com/connect?response_type=code&client_id=SrSLyYuwnffktH2oGJEJbQTiCXzkHgoL&redirect_uri=override-url&state=YXYdMjx6zDIdw3rxBwAXCocwhShDmcr3fN1NLrJHma6PM+jRwdzIMTqbo8xfiRaTY2aGlqv5u7Re8O82T1tzpqqpJhr3ORirkJK6JL3DBmw=";

            IEnumerable<KeyValuePair<string, string>> queryStringKeyValuePairs = queryString.GetQueryParams(true);

            List<KeyValuePair<string, string>> expectedKeyValuePairs = new()
            {
                new KeyValuePair<string, string>("response_type", "code"),
                new KeyValuePair<string, string>("client_id", "SrSLyYuwnffktH2oGJEJbQTiCXzkHgoL"),
                new KeyValuePair<string, string>("redirect_uri", "override-url"),
                new KeyValuePair<string, string>("state", "YXYdMjx6zDIdw3rxBwAXCocwhShDmcr3fN1NLrJHma6PM+jRwdzIMTqbo8xfiRaTY2aGlqv5u7Re8O82T1tzpqqpJhr3ORirkJK6JL3DBmw=")
            };

            queryStringKeyValuePairs.Should().BeEquivalentTo(expectedKeyValuePairs);
        }

        [Fact]
        public void Test()
        {
            string str =
                "https://secure.vendhq.com/connect?response_type=code&client_id=SrSLyYuwnffktH2oGJEJbQTiCXzkHgoL&redirect_uri=override-url&state=YXYdMjx6zDIdw3rxBwAXCocwhShDmcr3fN1NLrJHma6PM+jRwdzIMTqbo8xfiRaTY2aGlqv5u7Re8O82T1tzpqqpJhr3ORirkJK6JL3DBmw=";

           string encodedUrl = HttpUtility.UrlEncode(str);
            
          NameValueCollection nvc =  HttpUtility.ParseQueryString( encodedUrl);
          
          
        }        
        [Fact]
        public void Can_remove_key_value_pair()
        {
            string queryString =
                "code=d5a16abfbe15965ddd272a37cdce8f68&hmac=d3364df5bb6541ad06e7ae51cf522f5567ce67b931f84ccab31711bea0c807d8&host=YWlyc2xpcC1kZXZlbG9wbWVudC5teXNob3BpZnkuY29tL2FkbWlu&shop=airslip-development.myshopify.com&timestamp=1639585552&state=id";

            List<KeyValuePair<string, string>> queryStringKeyValuePairs = queryString.GetQueryParams().ToList();

            KeyValuePair<string, string> hmacKeyValuePair = queryStringKeyValuePairs.Get("hmac");

            queryStringKeyValuePairs.Remove(hmacKeyValuePair);
            
            List<KeyValuePair<string, string>> expectedKeyValuePairs = new()
            {
                new KeyValuePair<string, string>("code", "d5a16abfbe15965ddd272a37cdce8f68"),
                new KeyValuePair<string, string>("host", "YWlyc2xpcC1kZXZlbG9wbWVudC5teXNob3BpZnkuY29tL2FkbWlu"),
                new KeyValuePair<string, string>("shop", "airslip-development.myshopify.com"),
                new KeyValuePair<string, string>("state", "id"),
                new KeyValuePair<string, string>("timestamp", "1639585552")
            };

            queryStringKeyValuePairs.Should().BeEquivalentTo(expectedKeyValuePairs);
        }
    }
}
