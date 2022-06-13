using System;
using System.Globalization;
using System.Linq;

namespace Airslip.Common.Types
{
    public static class Culture
    {
        /// <summary>
        ///     Method used to return a currency symbol.
        ///     It receive as a parameter a currency code (3 digits).
        /// </summary>
        /// <param name="currencyCode">3 digits code. Samples GBP, BRL, USD, etc.</param>
        public static string GetCurrencySymbol(string currencyCode)
        {
            RegionInfo regionInfo = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(culture => culture.Name.Length > 0 && !culture.IsNeutralCulture)
                .Select(culture => new { culture, region = new RegionInfo(culture.Name) })
                .Where(t =>
                    string.Equals(t.region.ISOCurrencySymbol, currencyCode, StringComparison.InvariantCultureIgnoreCase))
                .Select(t => t.region).First();
            
            return regionInfo.CurrencySymbol;
        }
    }
}