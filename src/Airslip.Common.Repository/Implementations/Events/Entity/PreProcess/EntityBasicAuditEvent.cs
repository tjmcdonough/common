using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Entities;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;

public class EntityBasicAuditEvent<TEntity> : IEntityPreProcessEvent<TEntity> 
    where TEntity : class, IEntity
{
    private readonly IUserContext _userService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public EntityBasicAuditEvent(IUserContext userService, IDateTimeProvider dateTimeProvider)
    {
        _userService = userService;
        _dateTimeProvider = dateTimeProvider;
    }
        
    public IEnumerable<LifecycleStage> AppliesTo => new[]
        {LifecycleStage.Create, LifecycleStage.Delete, LifecycleStage.Update};
        
    public TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null)
    {
        if (!lifecycleStage.CheckApplies(AppliesTo)) return entity;
            
        entity.AuditInformation ??= new BasicAuditInformation
        {
            Id = CommonFunctions.GetId(),
            DateCreated = _dateTimeProvider.GetUtcNow(), // Defaults for new records
            CreatedByUserId = userId ?? _userService.UserId
        };

        switch (lifecycleStage)
        {
            case LifecycleStage.Create:
                entity.AuditInformation.DateCreated = _dateTimeProvider.GetUtcNow();
                entity.AuditInformation.CreatedByUserId = userId ?? _userService.UserId;
                break;
            case LifecycleStage.Update:
                entity.AuditInformation.DateUpdated = _dateTimeProvider.GetUtcNow();
                entity.AuditInformation.UpdatedByUserId = userId ?? _userService.UserId;
                break;
            case LifecycleStage.Delete:
                entity.AuditInformation.DateDeleted = _dateTimeProvider.GetUtcNow();
                entity.AuditInformation.DeletedByUserId = userId ?? _userService.UserId;
                break;
        }

        return entity;
    }
}