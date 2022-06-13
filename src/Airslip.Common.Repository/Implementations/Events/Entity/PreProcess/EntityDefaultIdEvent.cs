using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Utilities;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;

public class EntityDefaultIdEvent<TEntity> : IEntityPreProcessEvent<TEntity> 
    where TEntity : class, IEntity
{
    public IEnumerable<LifecycleStage> AppliesTo => new[]
        {LifecycleStage.Create};
        
    public TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null)
    {
        if (!lifecycleStage.CheckApplies(AppliesTo)) return entity;
            
        entity.Id = string.IsNullOrWhiteSpace(entity.Id) ? CommonFunctions.GetId() : entity.Id;

        return entity;
    }
}