using System;
using System.Globalization;
using System.Linq;

namespace Airslip.Common.Types
{
    public static class Currency
    {
        /// <summary>
        ///     Method used to return a currency symbol.
        ///     It receive as a parameter a currency code (3 digits).
        /// </summary>
        /// <param name="code">3 digits code. Samples GBP, BRL, USD, etc.</param>
        public static string GetSymbol(string code)
        {
            RegionInfo regionInfo = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(culture => culture.Name.Length > 0 && !culture.IsNeutralCulture)
                .Select(culture => new { culture, region = new RegionInfo(culture.Name) })
                .Where(t =>
                    string.Equals(t.region.ISOCurrencySymbol, code, StringComparison.InvariantCultureIgnoreCase))
                .Select(t => t.region).First();

            return regionInfo.CurrencySymbol;
        }

        private static long MakePositive(long negativeNumber) => Math.Abs(negativeNumber);
        private static decimal MakePositive(decimal negativeNumber) => Math.Abs(negativeNumber);
        private static long ConvertToLong(decimal value) => Convert.ToInt64(value * 100);
        private static long ConvertToLong(double value) => Convert.ToInt64(value * 100);

        public static long? ConvertToUnit(string? source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            bool canParse = double.TryParse(source, out double value);

            return canParse ? MakePositive(ConvertToUnit(value)!.Value) : MakePositive((long)value);
        }

        public static long? ConvertToUnit(decimal? value)
        {
            if (value is null)
                return null;

            return MakePositive(ConvertToLong(value.Value));
        }

        public static long? ConvertToUnit(double? value)
        {
            if (value is null)
                return null;

            return MakePositive(ConvertToLong(value.Value));
        }
        
        public static long? ConvertFivePlaceDecimalToUnit(double? value)
        {
            if (value is null)
                return null;
            
            double truncateToTwoDecimalPlaces = TruncateToTwoDecimalPlaces(value.Value);
            
            return MakePositive(ConvertToLong(truncateToTwoDecimalPlaces));
        }

        public static long? ConvertFivePlaceDecimalToUnit(string? value)
        {
            if (value is null)
                return null;
            
            double truncateToTwoDecimalPlaces = TruncateToTwoDecimalPlaces(Convert.ToDouble(value));

            return MakePositive(ConvertToLong(truncateToTwoDecimalPlaces));
        }

        public static decimal? ConvertToTwoPlacedDecimal(long? value)
        {
            if (value is null)
                return null;

            return MakePositive(Convert.ToDecimal(value) / 100);
        }

        private static double TruncateToTwoDecimalPlaces(double value)
        {
            double truncateToTwoDecimalPlaces = Math.Truncate(100 * value) / 100;
            return truncateToTwoDecimalPlaces;
        }
    }
}