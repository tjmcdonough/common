using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations;

public class RepositoryLifecycle<TEntity, TModel> : IRepositoryLifecycle<TEntity, TModel>
    where TEntity : class, IEntity 
    where TModel : class, IModel
{
    private readonly IEnumerable<IEntityPreProcessEvent<TEntity>> _entityPreProcessEvents;
    private readonly IEnumerable<IEntityPostProcessEvent<TEntity>> _entityPostProcessEvents;
    private readonly IEnumerable<IEntityPreValidateEvent<TEntity, TModel>> _entityPreValidateEvents;
    private readonly IEnumerable<IModelPostProcessEvent<TModel>> _modelPostProcessEvents;
    private readonly IEnumerable<IModelPreValidateEvent<TEntity, TModel>> _modelPreValidateEvents;

    public RepositoryLifecycle(IEnumerable<IEntityPreProcessEvent<TEntity>> entityPreProcessEvents,
        IEnumerable<IEntityPostProcessEvent<TEntity>> entityPostProcessEvents,
        IEnumerable<IModelPostProcessEvent<TModel>> modelPostProcessEvents,
        IEnumerable<IEntityPreValidateEvent<TEntity, TModel>> entityPreValidateEvents, 
        IEnumerable<IModelPreValidateEvent<TEntity, TModel>> modelPreValidateEvents)
    {
        _entityPreProcessEvents = entityPreProcessEvents;
        _entityPostProcessEvents = entityPostProcessEvents;
        _modelPostProcessEvents = modelPostProcessEvents;
        _entityPreValidateEvents = entityPreValidateEvents;
        _modelPreValidateEvents = modelPreValidateEvents;
    }
        
    public TEntity PreProcess(RepositoryAction<TEntity,TModel> repositoryAction)
    {
        if (repositoryAction.Entity == null)
            throw new ArgumentException("Entity cannot be null", nameof(repositoryAction));

        return _entityPreProcessEvents.Where(o => o.AppliesTo.Contains(repositoryAction.LifecycleStage))
            .Aggregate(repositoryAction.Entity, (current, preProcessEvent) => preProcessEvent
                .Execute(current, repositoryAction.LifecycleStage, repositoryAction.UserId));
    }

    public TEntity PostProcess(RepositoryAction<TEntity,TModel> repositoryAction)
    {
        if (repositoryAction.Entity == null)
            throw new ArgumentException("Entity cannot be null", nameof(repositoryAction));

        return _entityPostProcessEvents.Where(o => o.AppliesTo.Contains(repositoryAction.LifecycleStage))
            .Aggregate(repositoryAction.Entity, (current, postProcessEvent) => postProcessEvent
                .Execute(current, repositoryAction.LifecycleStage, repositoryAction.UserId));
    }

    public Task<TModel> PostProcessModel(RepositoryAction<TEntity,TModel> repositoryAction)
    {
        if (repositoryAction.Model == null)
            throw new ArgumentException("Model cannot be null", nameof(repositoryAction));
            
        return _processModel(repositoryAction.Model, repositoryAction.LifecycleStage,
            _modelPostProcessEvents
                .Where(o => o.AppliesTo.Contains(repositoryAction.LifecycleStage)));
    }
        
    private async Task<TModel> _processModel(TModel model, LifecycleStage lifecycleStage, IEnumerable<IModelProcessEvent<TModel>> events)
    {
        foreach (IModelProcessEvent<TModel> processEvent in events)
        {
            await processEvent.Execute(model, lifecycleStage);
        }

        return model;
    }

    public Task<ValidationResultModel> PreValidateModel(RepositoryAction<TEntity, TModel> repositoryAction)
    {
        return Validate(_modelPreValidateEvents, repositoryAction);
    }

    public Task<ValidationResultModel> PreValidateEntity(RepositoryAction<TEntity, TModel> repositoryAction)
    {
        return Validate(_entityPreValidateEvents, repositoryAction);
    }

    private static async Task<ValidationResultModel> Validate(
        IEnumerable<IValidationEvent<TEntity, TModel>> validationEvents, 
        RepositoryAction<TEntity, TModel> repositoryAction)
    {
        ValidationResultModel result = new();

        IEnumerable<IValidationEvent<TEntity, TModel>> validators = validationEvents
            .Where(o => o.AppliesTo.Contains(repositoryAction.LifecycleStage));

        foreach (IValidationEvent<TEntity, TModel> validator in validators)
        {
            List<ValidationResultMessageModel> validationResult = await validator
                .Validate(repositoryAction);
                
            result.Results.AddRange(validationResult);
        }
            
        return result;
    }
}