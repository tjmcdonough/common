using System;
using System.Linq;

namespace Airslip.Common.Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimeMilliseconds(this DateTime value)
        {
            return new DateTimeOffset(value).ToUnixTimeMilliseconds();
        }

        public static bool IsBetweenTwoDates(this long dt, long start, long end)
        {
            return dt >= start && dt <= end;
        }

        public static string ToIso8601(this DateTime datetime)
        {
            return DateTime.SpecifyKind(datetime, DateTimeKind.Utc).ToString("O");
        }

        public static string ToIso8601(this long datetime)
        {
            DateTimeOffset date = DateTimeOffset.FromUnixTimeMilliseconds(datetime);
            return DateTime.SpecifyKind(date.DateTime, DateTimeKind.Utc).ToString("O");
        }

        public static string ToIso8601(this DateTimeOffset datetime)
        {
            return datetime.ToString("O");
        }
        
        public static DateTime ToUtcDate(this long timestamp) =>
                    DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime.Date;
        
        public static string GetTime(this long timestamp) =>
            DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime.TimeOfDay.ToString();

        public static DateTimeOffset GetEarliestDate(params DateTimeOffset[] dates)
        {
            return dates.Min();
        }

        public static long GetEarliestDateInEpoch(params DateTimeOffset[] dates)
        {
            return GetEarliestDate(dates).ToUnixTimeMilliseconds();
        }

        public static int GetMonthsBetweenDates(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            double oneMonthExact = 365.25 / 12;
            return (int)Math.Round(endDate.Subtract(startDate).Days / oneMonthExact);
        }

        public static long GetTimestamp() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}