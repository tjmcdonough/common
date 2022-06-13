using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations.Events.Entity.PreValidate;

public class EntityStatusValidation<TEntity, TModel> : IEntityPreValidateEvent<TEntity, TModel> 
    where TEntity : class, IEntity 
    where TModel : class, IModel
{
    public IEnumerable<LifecycleStage> AppliesTo => new[]
        { LifecycleStage.Get };

    public Task<List<ValidationResultMessageModel>> Validate(RepositoryAction<TEntity, TModel> repositoryAction)
    {
        List<ValidationResultMessageModel> result = new(); 
        result.AddRange(repositoryAction.CheckApplies(AppliesTo));
        
        if (repositoryAction.Entity is {EntityStatus: EntityStatus.Deleted})
        {
            // If not, return a not found message
            result.Add(new ValidationResultMessageModel(nameof(repositoryAction.Entity.Id), 
                ErrorMessages.EntityDeleted)
            {
                ErrorCode = ErrorCodes.NotFound
            });
        }

        return Task.FromResult(result);
    }
}