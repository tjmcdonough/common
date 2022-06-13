using Airslip.Common.Types.Enums;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Airslip.Common.Auth.Functions.Data;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public record ApiAccessOptions(IServiceCollection Services, ApiAccessRights ApiAccessRights)
{
            
    /// <summary>
    /// Add access rights for use across all http functions  
    /// </summary>
    /// <param name="allowedTypes">The types allowed for this api</param>
    /// <param name="allowedEntities">The entities allowed for this api - this would
    /// generally be a name given to a particular application and provided in the EntityId field of
    /// the ApiKey</param>
    /// <returns>An ApiAccessOptions class for chaining</returns>
    public ApiAccessOptions AddGeneralAccessRights(List<AirslipUserType> allowedTypes,
        List<string> allowedEntities)
    {
        ApiAccessRights.AddGeneralAccessRights(allowedTypes, allowedEntities);
        return this;
    }
    
    /// <summary>
    /// Add access rights to match on a string based name, this would
    /// generally be used to match for function names
    /// </summary>
    /// <param name="named">The name of the function being executed</param>
    /// <param name="allowedTypes">The types allowed for this named function</param>
    /// <param name="allowedEntities">The entities allowed for this named function - this would
    /// generally be a name given to a particular application and provided in the EntityId field of
    /// the ApiKey</param>
    /// <returns>An ApiAccessOptions class for chaining</returns>
    public ApiAccessOptions AddNamedAccessRights(string named, List<AirslipUserType> allowedTypes,
        List<string> allowedEntities)
    {
        ApiAccessRights.AddNamedAccessRights(named, allowedTypes, allowedEntities);
        return this;
    }

    /// <summary>
    /// Add access rights to match on a string based name, this would
    /// generally be used to match for function names
    /// </summary>
    /// <param name="named">The name of the function being executed</param>
    /// <param name="allowedType">The type allowed for this named function</param>
    /// <param name="allowedEntity">The entity allowed for this named function - this would
    /// generally be a name given to a particular application and provided in the EntityId field of
    /// the ApiKey</param>
    /// <returns>An ApiAccessOptions class for chaining</returns>
    public ApiAccessOptions AddNamedAccessRights(string named, AirslipUserType? allowedType, string? allowedEntity)
    {
        List<AirslipUserType> allowedTypes = new();
        if (allowedType != null) allowedTypes.Add(allowedType.Value);
        
        List<string> allowedEntities = new();
        if (allowedEntity != null) allowedEntities.Add(allowedEntity);
        
        return AddNamedAccessRights(named,
            allowedTypes,
            allowedEntities);
    }

    /// <summary>
    /// Add access rights to match on a string based name, this would
    /// generally be used to match for function names
    /// </summary>
    /// <param name="named">The name of the function being executed</param>
    /// <param name="allowedType">The type allowed for this named function</param>
    /// <returns>An ApiAccessOptions class for chaining</returns>
    public ApiAccessOptions AddNamedAccessRights(string named, AirslipUserType allowedType)
    {
        return AddNamedAccessRights(named,
            new List<AirslipUserType> {allowedType},
            new List<string>());
    }

    /// <summary>
    /// Add access rights to match on a string based name, this would
    /// generally be used to match for function names
    /// </summary>
    /// <param name="named">The name of the function being executed</param>
    /// <param name="allowedEntity">The entity allowed for this named function - this would
    ///     generally be a name given to a particular application and provided in the EntityId field of
    ///     the ApiKey</param>
    /// <returns>An ApiAccessOptions class for chaining</returns>
    public ApiAccessOptions AddNamedAccessRights(string named, string allowedEntity)
    {
        return AddNamedAccessRights(named,
            new List<AirslipUserType>(),
            new List<string> {allowedEntity});
    }
}