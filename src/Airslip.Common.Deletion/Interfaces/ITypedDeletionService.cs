using Airslip.Common.Deletion.Models;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Interfaces;
using System.Threading.Tasks;

namespace Airslip.Common.Deletion.Interfaces;

public interface ITypedDeletionService<in TModel> : IDeletionService
    where TModel : class, IModel
{
    Task<IResponse> DeleteRecord(string integration, TModel model, DeleteRequest requestDetails);
}