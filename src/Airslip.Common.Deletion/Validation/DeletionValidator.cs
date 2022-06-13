using Airslip.Common.Deletion.Models;
using Airslip.Common.Services.FluentValidation;
using Airslip.Common.Types.Enums;
using FluentValidation;

namespace Airslip.Common.Deletion.Validation;

public class DeletionValidator : ModelValidatorBase<DeleteRequest>
{
    public DeletionValidator()
    {
        RuleFor(o => o.EntityId)
            .NotEmpty();

        RuleFor(o => o.UserId)
            .NotEmpty();

        RuleFor(o => o.AirslipUserType)
            .NotEqual(AirslipUserType.Standard)
            .NotEqual(AirslipUserType.Unknown)
            .NotEqual(AirslipUserType.Unregistered)
            .NotEqual(AirslipUserType.InternalApi)
            .NotEqual(AirslipUserType.Administrator);
    }
}