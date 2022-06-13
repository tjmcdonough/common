using Airslip.Common.Auth.Functions.Configuration;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Types.Enums;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Airslip.Common.Auth.Functions.Data;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class ApiAccessRights : IApiAccessRights
{
    public List<ApiAccessDefinition> AccessDefinitions { get; } = new();

    // Legacy support for existing settings...
    public void AddFromSettings(IConfiguration configuration)
    {
        IConfigurationSection? configurationSection = configuration
            .GetSection(nameof(ApiAccessSettings));

        ApiAccessSettings? apiSettings = configurationSection?
            .Get<ApiAccessSettings>();
            
        if (apiSettings != null) 
            AccessDefinitions
                .Add(new ApiAccessDefinition(apiSettings.AllowedTypes, apiSettings.AllowedEntities));
    }

    public void AddNamedAccessRights(string named, List<AirslipUserType> allowedTypes,
        List<string> allowedEntities)
    {
        AccessDefinitions.Add(new ApiAccessDefinition(allowedTypes, allowedEntities, named));
    }

    public void AddGeneralAccessRights(List<AirslipUserType> allowedTypes, List<string> allowedEntities)
    {
        AccessDefinitions.Add(new ApiAccessDefinition(allowedTypes, allowedEntities));
    }
}