using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Exception;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;

public class EntityOwnershipEvent<TEntity> : IEntityPreProcessEvent<TEntity> 
    where TEntity : class, IEntity
{
    private readonly IUserContext _userService;

    public EntityOwnershipEvent(IUserContext userService)
    {
        _userService = userService;
    }
        
    public IEnumerable<LifecycleStage> AppliesTo => new[]
        {LifecycleStage.Create};
        
    public TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null)
    {
        if (!lifecycleStage.CheckApplies(AppliesTo)) return entity;
            
        if (entity is not IEntityWithOwnership entityWithOwnership) 
            return entity;
            
        if (_userService.AirslipUserType is null)
        {
            throw new RepositoryLifecycleException(ErrorCodes.VerificationFailed, false);
        }
                
        entityWithOwnership.AirslipUserType =
            entityWithOwnership.AirslipUserType == AirslipUserType.Unknown ?
                _userService.AirslipUserType.Value : entityWithOwnership.AirslipUserType;

        // Bind the UserId and EntityId where available
        switch (_userService.AirslipUserType.Value)
        { 
            case AirslipUserType.Standard:
                entityWithOwnership.UserId ??= _userService.UserId;
                break;
                    
            default:
                entityWithOwnership.UserId ??= _userService.UserId;
                entityWithOwnership.EntityId ??= _userService.EntityId;
                break;
        }

        return entity;
    }
}