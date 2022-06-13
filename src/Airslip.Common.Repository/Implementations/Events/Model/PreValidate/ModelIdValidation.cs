using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations.Events.Model.PreValidate;

public class ModelIdValidation<TEntity, TModel> : IModelPreValidateEvent<TEntity, TModel> 
    where TModel : class, IModel 
    where TEntity : class, IEntity
{
    public IEnumerable<LifecycleStage> AppliesTo => new[]
        { LifecycleStage.Update };

    public Task<List<ValidationResultMessageModel>> Validate(RepositoryAction<TEntity, TModel> repositoryAction)
    {
        List<ValidationResultMessageModel> result = new(); 
        result.AddRange(repositoryAction.CheckApplies(AppliesTo));

        if (repositoryAction.Id is null || 
            repositoryAction.Model is null || 
            !repositoryAction.Id.Equals(repositoryAction.Model.Id))
        {
            result.Add(new ValidationResultMessageModel(nameof(repositoryAction.Id), 
                ErrorMessages.IdFailedVerification)
            {
                ErrorCode = ErrorCodes.VerificationFailed
            });
        }

        return Task.FromResult(result);
    }
}