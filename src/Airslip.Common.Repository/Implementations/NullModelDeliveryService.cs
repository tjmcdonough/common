using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations;

public class NullModelDeliveryService<TModel> : IModelDeliveryService<TModel> 
    where TModel : class, IModel
{
    public Task Deliver(TModel model)
    {
        return Task.FromResult(true);
    }
}