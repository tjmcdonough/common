using Airslip.Common.Types.Enums;
using System.Collections.Generic;

namespace Airslip.Common.Auth.Functions.Data;

public record ApiAccessDefinition(List<AirslipUserType> AllowedTypes,
    List<string> AllowedEntities, string? Named = null);