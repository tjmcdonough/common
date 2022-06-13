using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Utilities.Extensions;
using System;

namespace Airslip.Common.Repository.Implementations;

public class DateTimeProvider : IDateTimeProvider
{
    public long GetCurrentUnixTime()
    {
        return DateTime.UtcNow.ToUnixTimeMilliseconds();
    }

    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }
}