using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations;

public class NullEntitySearchFormatter<TModel> : IEntitySearchFormatter<TModel> 
    where TModel : class, IModel
{
    public async Task<TModel> FormatModel(TModel model)
    {
        return await Task.FromResult(model);
    }
}