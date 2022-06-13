using System;

namespace Airslip.Common.Repository.Interfaces;

public interface IDateTimeProvider
{
    long GetCurrentUnixTime();
    DateTime GetUtcNow();
}