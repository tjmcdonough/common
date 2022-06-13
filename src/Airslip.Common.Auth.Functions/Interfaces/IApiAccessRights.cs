using Airslip.Common.Auth.Functions.Data;
using Airslip.Common.Types.Enums;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Airslip.Common.Auth.Functions.Interfaces;

public interface IApiAccessRights
{
    List<ApiAccessDefinition> AccessDefinitions { get; }
    void AddFromSettings(IConfiguration configuration);

    void AddNamedAccessRights(string named, List<AirslipUserType> allowedTypes,
        List<string> allowedEntities);

    void AddGeneralAccessRights(List<AirslipUserType> allowedTypes, List<string> allowedEntities);
}