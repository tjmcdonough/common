using Airslip.Common.Deletion.Interfaces;
using Airslip.Common.Deletion.Models;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Interfaces;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Airslip.Common.Deletion.Implementations;

[UsedImplicitly]
public abstract class TypedDeletionBase<TModel> : DeletionBase, ITypedDeletionService<TModel>
    where TModel : class, IModel
{
    protected TypedDeletionBase(IModelValidator<DeleteRequest> validator) : base(validator)
    {
        
    }

    public virtual async Task<IResponse> DeleteRecord(string integration, TModel model, DeleteRequest requestDetails)
    {
        return await ValidateDeletion(requestDetails);
    }
}