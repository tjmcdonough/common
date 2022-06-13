using Airslip.Common.Repository.Types.Models;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Types.Interfaces;

/// <summary>
/// Generic repository definition for common CRUD functions, uses a design pattern which doesn't directly
///  expose database entities to APIs but hides them behind Model classes
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TModel">The model type</typeparam>
public interface IRepository<TEntity, TModel> 
    where TEntity : class, IEntity 
    where TModel : class, IModel
{
        
    /// <summary>
    /// Add an entry to the context
    /// </summary>
    /// <param name="model">The model to add</param>
    /// <param name="userId">Optional User Id overrides value held internally</param>
    /// <returns>A response model containing any validation results, and the new model if successfully created</returns>
    Task<RepositoryActionResultModel<TModel>> Add(TModel model, string? userId = null);

    /// <summary>
    /// Updates an existing entry in the context
    /// </summary>
    /// <param name="id">The id of the entry to be update, must match the id on the model</param>
    /// <param name="model">The model with updated data</param>
    /// <param name="userId">Optional User Id overrides value held internally</param>
    /// <returns>A response model containing any validation results with previous and current versions of the model if successfully updated</returns>
    Task<RepositoryActionResultModel<TModel>> Update(string id, TModel model, string? userId = null);

    /// <summary>
    /// Creates or updates entry in the context
    /// </summary>
    /// <param name="id">The id of the entry to be update, must match the id on the model</param>
    /// <param name="model">The model with updated data</param>
    /// <param name="userId">Optional User Id overrides value held internally</param>
    /// <returns>A response model containing any validation results with previous and current versions of the model if successfully updated</returns>
    Task<RepositoryActionResultModel<TModel>> Upsert(string id, TModel model, string? userId = null);

    /// <summary>
    /// Marks an existing entry as deleted
    /// </summary>
    /// <param name="id">The id to mark as deleted</param>
    /// <param name="userId">Optional User Id overrides value held internally</param>
    /// <returns>A response model containing any validation results with previous version of the model if successfully deleted</returns>
    Task<RepositoryActionResultModel<TModel>> Delete(string id, string? userId = null);

    /// <summary>
    /// Lookup a particular entity by Id 
    /// </summary>
    /// <param name="id">The id of the entity to return</param>
    /// <returns>A response model containing the current version of the model if successfully found</returns>
    Task<RepositoryActionResultModel<TModel>> Get(string id);
}