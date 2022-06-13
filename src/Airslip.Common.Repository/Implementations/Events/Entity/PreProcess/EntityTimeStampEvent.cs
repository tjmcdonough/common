using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Exception;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;

public class EntityTimeStampEvent<TEntity> : IEntityPreProcessEvent<TEntity> 
    where TEntity : class, IEntity
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public EntityTimeStampEvent(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }
    
    public IEnumerable<LifecycleStage> AppliesTo => new[]
        { LifecycleStage.Create, LifecycleStage.Update, LifecycleStage.Delete };
        
    public TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null)
    {
        if (!lifecycleStage.CheckApplies(AppliesTo)) return entity;
            
        if (entity is not IEntityWithTimeStamp entityWithTimestamp) 
            return entity;

        entityWithTimestamp.TimeStamp = _dateTimeProvider.GetCurrentUnixTime();

        return entity;
    }
}