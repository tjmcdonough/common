using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Airslip.Common.Security.Implementations
{
    public static class HmacCipher
    {
        public static bool Validate(IEnumerable<KeyValuePair<string, string>> queryStringValuePairs, string hmac, string secretKey)
        {
            string queryString = PrepareQuerystring(queryStringValuePairs, "&");
            HMACSHA256 hmacHasher = new(Encoding.UTF8.GetBytes(secretKey));
            byte[] hash = hmacHasher.ComputeHash(Encoding.UTF8.GetBytes(string.Join("&", queryString)));
            string calculatedSignature = BitConverter.ToString(hash).Replace("-", ""); 
            
            return calculatedSignature.ToUpper() == hmac.ToUpper();
        }
        
        private static string PrepareQuerystring(IEnumerable<KeyValuePair<string, string>> querystring, string joinWith)
        {
            IEnumerable<string> kvps = querystring.Select(kvp => new
                {
                    Key = EncodeQuery(kvp.Key, kvp.Value, true),
                    Value = EncodeQuery(kvp.Key, kvp.Value, false)
                })
                .OrderBy(kvp => kvp.Key, StringComparer.Ordinal)
                .Select(kvp => $"{kvp.Key}={kvp.Value}");

            return string.Join(joinWith, kvps);
        }
        
        private static string EncodeQuery(string key, StringValues values, bool isKey)
        {
            string? result;

            if (isKey)
                result = key;
            else
            {
                result = values.Count <= 1 && !key.EndsWith("[]") ?
                    values.FirstOrDefault() :
                    '[' + string.Join(", ", values.Select(v => '"' + v + '"')) + ']';
            }

            if (string.IsNullOrEmpty(result))
                return string.Empty;

            // Replacements for decoding a HMAC
            // Replace % before replacing &. Else second replace will replace those %25s.
            result = result.Replace("%", "%25").Replace("&", "%26");

            if (isKey)
                result = result.Replace("=", "%3D").Replace("[]", "");

            return result;
        }
    }
}