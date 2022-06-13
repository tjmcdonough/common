using Airslip.Common.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Airslip.Common.Utilities
{
    public static class Json
    {
        public const string MediaType = "application/json";

        public static string Serialize(object value,
            Casing casing = Casing.CAMEL_CASE,
            Formatting formatting = Formatting.None)
        {
            return Serialize(value, casing, formatting, NullValueHandling.Include);
        }

        public static string Serialize(object value,
            Casing casing,
            Formatting formatting,
            NullValueHandling nullValueHandling)
        {
            JsonSerializerSettings jsonSerializerSettings =
                GetJsonSerializerSettings(casing, formatting, nullValueHandling);

            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }

        public static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value) ??
                   throw new InvalidOperationException("Value deserialized to null");
        }

        public static T Deserialize<T>(string value,
            Casing casing,
            Formatting formatting = Formatting.None,
            NullValueHandling nullValueHandling = NullValueHandling.Include)
        {
            JsonSerializerSettings jsonSerializerSettings =
                GetJsonSerializerSettings(casing, formatting, nullValueHandling);

            return JsonConvert.DeserializeObject<T>(value, jsonSerializerSettings) ??
                   throw new InvalidOperationException("Value deserialized to null");
        }

        private static JsonSerializerSettings GetJsonSerializerSettings(
            Casing casing,
            Formatting formatting,
            NullValueHandling nullValueHandling)
        {
            return casing switch
            {
                Casing.CAMEL_CASE => new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = formatting,
                    NullValueHandling = nullValueHandling
                },
                Casing.SNAKE_CASE => new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    Formatting = formatting,
                    NullValueHandling = nullValueHandling
                },
                _ => new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = formatting,
                    NullValueHandling = nullValueHandling
                }
            };
        }
    }
}