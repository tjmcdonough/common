using Airslip.Common.Utilities.Extensions;
using FluentAssertions;
using System;
using Xunit;

namespace Airslip.Common.Utilities.UnitTests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void Can_get_earliest_date_in_epoch()
        {
            DateTimeOffset midDate = DateTimeOffset.Parse("2019-12-31");
            DateTimeOffset minDate = DateTimeOffset.Parse("2018-01-01");
            DateTimeOffset maxDate = DateTimeOffset.Parse("2020-02-13");

            DateTimeOffset[] dates = new DateTimeOffset[3];
            dates[0] = midDate;
            dates[1] = minDate;
            dates[2] = maxDate;

            long dateInEpoch = DateTimeExtensions.GetEarliestDateInEpoch(dates);
            dateInEpoch.Should().Be(1514764800000);
        }
        
        [Fact]
        public void Can_get_earliest_date_from_collection()
        {
            DateTimeOffset midDate = DateTimeOffset.Parse("2019-12-31");
            DateTimeOffset maxDate = DateTimeOffset.Parse("2020-02-13");
            DateTimeOffset minDate = DateTimeOffset.Parse("2018-01-01");

            DateTimeOffset[] dates = new DateTimeOffset[3];
            dates[0] = midDate;
            dates[1] = maxDate;
            dates[2] = minDate;

            DateTimeOffset date = DateTimeExtensions.GetEarliestDate(dates);
            date.Should().Be(new DateTimeOffset(new DateTime(2018, 01,01)));
        }
        
        [Fact]
        public void Can_get_months_between_dates()
        {
            DateTimeOffset startDate = DateTimeOffset.Parse("2018-02-28");
            DateTimeOffset endDate = DateTimeOffset.Parse("2020-02-13");

            int monthCount = DateTimeExtensions.GetMonthsBetweenDates(startDate, endDate);
            monthCount.Should().Be(23);
        }
        
        [Fact]
        public void Can_convert_datetime_offset_to_iso8601()
        {
            DateTimeOffset datetime = DateTimeOffset.FromUnixTimeMilliseconds(1635325854776);
            string iso8601Date = datetime.ToIso8601();
            string expectedDate = "2021-10-27T09:10:54.7760000+00:00";
            iso8601Date.Should().Be(expectedDate);
        }
        
        [Fact]
        public void Can_convert_datetime_to_iso8601()
        {
            DateTime datetime = new(2021, 10, 27, 09, 10, 54,776);
            string iso8601Date = datetime.ToIso8601();
            string expectedDate = "2021-10-27T09:10:54.7760000Z";
            iso8601Date.Should().Be(expectedDate);
        }

        [Fact]
        public void Can_get_utc_date_only_from_timestamp()
        {
            // Tuesday, 26 October 2021 23:59:30
            long timestamp = 1635292770000;
            
            DateTime utcDate = timestamp.ToUtcDate();
            DateTime expectedDate = new(2021,10,26);
            utcDate.Should().Be(expectedDate);
        }
        
        [Fact]
        public void Can_get_utc_time_only_from_timestamp()
        {
            // Tuesday, 26 October 2021 23:59:30
            long timestamp = 1635292770000;
            
            string time = timestamp.GetTime();
            
            time.Should().Be("23:59:30");
        }
    }
}