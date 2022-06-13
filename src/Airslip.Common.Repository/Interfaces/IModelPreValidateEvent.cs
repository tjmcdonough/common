using Airslip.Common.Repository.Types.Interfaces;

namespace Airslip.Common.Repository.Interfaces;

public interface IModelPreValidateEvent<TEntity, TModel> : IValidationEvent<TEntity, TModel>
    where TEntity : class, IEntity 
    where TModel : class, IModel
{

}