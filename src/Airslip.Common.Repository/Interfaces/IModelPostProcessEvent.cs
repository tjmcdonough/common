using Airslip.Common.Repository.Types.Interfaces;

namespace Airslip.Common.Repository.Interfaces;

public interface IModelPostProcessEvent<TModel> : IModelProcessEvent<TModel>
    where TModel : class, IModel
{
    
}