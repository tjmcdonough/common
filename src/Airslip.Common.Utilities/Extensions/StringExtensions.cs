using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Airslip.Common.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static long ToUnixTimeMilliseconds(this string value)
        {
            return DateTimeOffset.Parse(value, DateTimeFormatInfo.InvariantInfo).ToUnixTimeMilliseconds();
        }

        public static bool IsInArray(this string subject, params string[] arr)
        {
            return arr
                .Select(item => subject.Equals(item, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault(isInArray => isInArray);
        }

        public static bool CheckIsUrl(this string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        public static bool TryParseUtcDateTime(this string datetimeString)
        {
            return DateTime.TryParseExact(datetimeString, "o", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out _);
        }

        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }

        public static string RemoveAccents(this string accentedStr)
        {
            byte[] tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            return Encoding.UTF8.GetString(tempBytes);
        }
        
        public static Stream ToStream(this string s)
        {
            return s.ToStream(Encoding.UTF8);
        }

        public static Stream ToStream(this string s, Encoding encoding)
        {
            return new MemoryStream(encoding.GetBytes(s));
        }
        
        public static IEnumerable<string> SplitBy(this string value, string character)
        {
            List<string> splitArray = value.Split(character).ToList();

            return splitArray.Select(s => s.Trim());
        }
        
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0)
                return text;

            return text[..pos] + replace + text[(pos + search.Length)..];
        }
        
        public static bool TryParseIgnoreCase<TEnum>(this string value, out TEnum parsedObject) where TEnum : struct
        {
            return Enum.TryParse(value, true, out parsedObject);
        }
        
        public static TEnum ParseIgnoreCase<TEnum>(this string value) where TEnum : struct
        {
            return Enum.Parse<TEnum>(value, true);
        }
        
        public static string Base64Encode(this string plainText) {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        
        public static string Base64Decode(this string base64EncodedData) {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}