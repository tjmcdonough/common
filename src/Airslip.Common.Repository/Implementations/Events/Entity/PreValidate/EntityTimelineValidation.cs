using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations.Events.Entity.PreValidate;

public class EntityTimelineValidation<TEntity, TModel> : IEntityPreValidateEvent<TEntity, TModel> 
    where TEntity : class, IEntity 
    where TModel : class, IModel
{
    public IEnumerable<LifecycleStage> AppliesTo => new[]
        {LifecycleStage.Update};

    public Task<List<ValidationResultMessageModel>> Validate(RepositoryAction<TEntity, TModel> repositoryAction)
    {
        List<ValidationResultMessageModel> result = new(); 
        result.AddRange(repositoryAction.CheckApplies(AppliesTo));
        
        if (repositoryAction.Model is not IFromDataSource dataSourceModel || 
            repositoryAction.Entity is not IFromDataSource dataSourceEntity) 
            return Task.FromResult(result);
            
        // Return false if entity is newer than model
        if (dataSourceEntity.TimeStamp >= dataSourceModel.TimeStamp)
            result.Add(new ValidationResultMessageModel(nameof(IFromDataSource.TimeStamp),
                ErrorMessages.ModelOutdated)
            {
                ErrorCode = ErrorCodes.ModelOutdated
            });

        return Task.FromResult(result);
    }
}