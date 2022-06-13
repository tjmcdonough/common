using System;

namespace Airslip.Common.Utilities.Extensions;

public static class NumericExtensions
{
    // Decimal converters
    public static long ConvertToLong(this decimal value) => Convert.ToInt64(value * 100);
    public static long? ConvertToLong(this decimal? value) => value?.ConvertToLong();
    public static long? ConvertToPositiveLong(this decimal? value) => value?.ConvertToPositiveLong();
    public static long ConvertToPositiveLong(this decimal value) => value.ConvertToLong().MakePositive();
    public static decimal? ConvertToPositive(this decimal? value) => value?.ConvertToPositive();
    public static decimal ConvertToPositive(this decimal value) => value.MakePositive();
    
    // Double converters
    public static long ConvertToLong(this double value) => Convert.ToInt64(value * 100);
    public static long? ConvertToLong(this double? value) => value?.ConvertToLong();
    public static long? ConvertToPositiveLong(this double? value) => value?.ConvertToPositiveLong();
    public static long ConvertToPositiveLong(this double value) => value.ConvertToLong().MakePositive();
    public static double? ConvertToPositive(this double? value) => value?.ConvertToPositive();
    public static double ConvertToPositive(this double value) => value.MakePositive();

    // String converters
    public static long? ConvertToLong(this string? value) => value?._convertToLong();
    private static long _convertToLong(this string source) => double.TryParse(source, out double value) ? 
        value.ConvertToLong() : (long)value;
    public static long? ConvertToPositiveLong(this string? source) => source?._convertToPositiveLong();
    private static long _convertToPositiveLong(this string source) => double.TryParse(source, out double value) ? 
        value.ConvertToPositiveLong() : (long)value;
    public static double? ConvertToPositive(this string? value) => value?._convertToPositive();
    private static double _convertToPositive(this string source) => double.TryParse(source, out double value) ? 
        value.ConvertToPositive() : (long)value;
    
    // Long converters
    public static long? ConvertToPositive(this long? value) => value?.ConvertToPositive();
    public static long ConvertToPositive(this long value) => value.MakePositive();
    
    // General
    public static long MakePositive(this long value) => value < 0 ? value * -1 : value;
    public static double MakePositive(this double value) => value < 0 ? value * -1 : value;
    public static decimal MakePositive(this decimal value) => value < 0 ? value * -1 : value;
}




