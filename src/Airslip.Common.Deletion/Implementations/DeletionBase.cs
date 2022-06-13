using Airslip.Common.Deletion.Interfaces;
using Airslip.Common.Deletion.Models;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.FluentValidation;
using Airslip.Common.Types;
using Airslip.Common.Types.Failures;
using Airslip.Common.Types.Interfaces;
using System.Threading.Tasks;

namespace Airslip.Common.Deletion.Implementations;

public abstract class DeletionBase : IDeletionService
{
    protected readonly IModelValidator<DeleteRequest> _validator;

    protected DeletionBase(IModelValidator<DeleteRequest> validator)
    {
        _validator = validator;
    }

    public virtual async Task<IResponse> DeleteRecord(string integration, string id, DeleteRequest requestDetails)
    {
        return await ValidateDeletion(requestDetails);
    }
    
    protected virtual async Task<IResponse> ValidateDeletion(DeleteRequest requestDetails)
    {
        IResponse validationResponse = await ValidatorService.ValidateUpdate(_validator, requestDetails);
        
        if (validationResponse is ErrorResponses errors)
        {
            return errors;
        }

        return Success.Instance;
    }
}