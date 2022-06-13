using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Types.Interfaces;

namespace Airslip.Common.Repository.Data;

public record RepositoryAction<TEntity, TModel>(string? Id, TEntity? Entity, TModel? Model,
    LifecycleStage LifecycleStage, string? UserId)
    where TEntity : class, IEntity
    where TModel : class, IModel
{
    public TEntity? Entity { get; private set; } = Entity;
    public TModel? Model { get; private set; } = Model;
    public LifecycleStage LifecycleStage { get; private set; } = LifecycleStage;

    public void SetEntity(TEntity? entity) => Entity = entity;
    
    public void SetModel(TModel? model) => Model = model;
    
    public void SetLifecycle(LifecycleStage lifecycleStage) => LifecycleStage = lifecycleStage;
};
