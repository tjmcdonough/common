using System.Collections.Generic;

namespace Airslip.Common.Utilities.Extensions
{
    public static class DictionaryExtensions
    {
        public static string? GetValue(this IDictionary<string, string> dictionary, string value)
        {
            dictionary.TryGetValue(value, out string? s);

            return s;
        }
        
        public static object? GetValue(this IDictionary<string, object> dictionary, string value)
        {
            dictionary.TryGetValue(value, out object? s);

            return s;
        }
        
        public static Dictionary<string, TValue> ToDictionary<TValue>(this object obj)
        {       
            string json = Json.Serialize(obj);
            Dictionary<string, TValue> dictionary = Json.Deserialize<Dictionary<string, TValue>>(json);   
            return dictionary;
        }
    }
}