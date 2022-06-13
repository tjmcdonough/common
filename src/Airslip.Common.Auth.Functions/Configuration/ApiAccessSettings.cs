using Airslip.Common.Types.Enums;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Airslip.Common.Auth.Functions.Configuration
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal record ApiAccessSettings
    {
        public List<AirslipUserType> AllowedTypes { get; init; } = new();
        public List<string> AllowedEntities { get; init; } = new();
    }
}