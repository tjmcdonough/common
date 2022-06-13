using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations.Events.Model.PreValidate;

public class ModelCreateValidation<TEntity, TModel> : IModelPreValidateEvent<TEntity, TModel> 
    where TModel : class, IModel 
    where TEntity : class, IEntity
{
    private readonly IModelValidator<TModel> _validator;

    public ModelCreateValidation(IModelValidator<TModel> validator)
    {
        _validator = validator;
    }
    
    public IEnumerable<LifecycleStage> AppliesTo => new[]
        { LifecycleStage.Create };

    public async Task<List<ValidationResultMessageModel>> Validate(RepositoryAction<TEntity, TModel> repositoryAction)
    {
        List<ValidationResultMessageModel> result = new();
        result.AddRange(repositoryAction.CheckApplies(AppliesTo));
        
        if (repositoryAction.Model == null) 
            return result;
        
        result.AddRange((await _validator.ValidateAdd(repositoryAction.Model)).Results);
        
        return result;
    }
}