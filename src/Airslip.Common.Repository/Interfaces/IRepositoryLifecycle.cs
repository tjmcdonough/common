using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Interfaces;

public interface IRepositoryLifecycle<TEntity, TModel> 
    where TEntity : class, IEntity 
    where TModel : class, IModel
{
    TEntity PreProcess(RepositoryAction<TEntity,TModel> repositoryAction);
    TEntity PostProcess(RepositoryAction<TEntity,TModel> repositoryAction);
    Task<TModel> PostProcessModel(RepositoryAction<TEntity,TModel> repositoryAction);
    Task<ValidationResultModel> PreValidateModel(RepositoryAction<TEntity,TModel> repositoryAction);
    Task<ValidationResultModel> PreValidateEntity(RepositoryAction<TEntity,TModel> repositoryAction);
}