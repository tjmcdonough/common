using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Interfaces;

public interface IValidationEvent<TEntity, TModel>
    where TEntity : class, IEntity 
    where TModel : class, IModel
{
    IEnumerable<LifecycleStage> AppliesTo { get; }
    Task<List<ValidationResultMessageModel>> Validate(RepositoryAction<TEntity, TModel> repositoryAction);
}