using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Utilities.Extensions
{
    public static class UtilityExtensions
    {
        public static bool InList<TType>(this TType value, params TType[] values)
        {
            return ((IList) values).Contains(value);
        }

        public static TReturnType GetSettingByName<TReturnType>(this SettingCollection<TReturnType> settings, string name)
        {
            KeyValuePair<string, TReturnType>? result = settings.Settings?
                .FirstOrDefault(o => o.Key.ToLower().Equals(name.ToLower()));
            
            if (result == null)
                throw new ArgumentException($"{nameof(TReturnType)}:Settings:{name} " +
                                            $"section missing from appSettings", name);

            return result.Value.Value;
        }

        public static PublicApiSetting GetSettingByName(this PublicApiSettings settings, string name)
        {
            return settings.GetSettingByName<PublicApiSetting>(name);
        }
        
        public static string ToBaseUri(this PublicApiSetting setting)
        {
            List<string> parts = new()
            {
                setting.BaseUri.RemoveLeadingAndTrailing("/"),
                setting.UriSuffix.RemoveLeadingAndTrailing("/"),
                setting.Version.RemoveLeadingAndTrailing("/")
            };
            parts.RemoveAll(string.IsNullOrWhiteSpace);
            return string.Join("/", parts);
        }

        private static string RemoveLeadingAndTrailing(this string fromValue, string removeValue)
        {
            if (fromValue.EndsWith(removeValue)) fromValue = fromValue.Remove(fromValue.Length - 1, 1);
            if (fromValue.StartsWith(removeValue)) fromValue = fromValue[1..];
            return fromValue;
        }
        
        public static T GetConfigurationSection<T>(IConfiguration configuration)
        {
            string className = typeof(T).Name;
            IConfigurationSection configurationSection = configuration.GetSection(className);
            return configurationSection.Get<T>();
        }
    }
}