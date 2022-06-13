using Airslip.Common.Types.Enums;
using System.Collections.Generic;

namespace Airslip.Common.Auth.AspNetCore.Configuration
{
    public record ApiAccessSettings
    {
        public List<AirslipUserType> AllowedTypes { get; init; } = new();
        public List<string> AllowedEntities { get; init; } = new();
    }
}