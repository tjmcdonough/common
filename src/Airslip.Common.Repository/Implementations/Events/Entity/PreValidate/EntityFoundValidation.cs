using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations.Events.Entity.PreValidate;

public class EntityFoundValidation<TEntity, TModel> : IEntityPreValidateEvent<TEntity, TModel> 
    where TEntity : class, IEntity 
    where TModel : class, IModel
{
    public IEnumerable<LifecycleStage> AppliesTo => new[]
        { LifecycleStage.Update, LifecycleStage.Get, LifecycleStage.Delete };

    public Task<List<ValidationResultMessageModel>> Validate(RepositoryAction<TEntity, TModel> repositoryAction)
    {
        List<ValidationResultMessageModel> result = new();
        result.AddRange(repositoryAction.CheckApplies(AppliesTo));
        
        if (repositoryAction.Entity == null)
        {
            // If not, return a not found message
            result.Add(new ValidationResultMessageModel(nameof(repositoryAction.Entity.Id), 
                ErrorMessages.NotFound)
            {
                ErrorCode = ErrorCodes.NotFound
            });
        }

        return Task.FromResult(result);
    }
}