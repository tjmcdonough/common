using Airslip.Common.Services.FluentValidation;
using Airslip.Common.Types.Validator;
using FluentValidation;

namespace Airslip.Common.Repository.UnitTests.Common;

public class MyModelValidator : ModelValidatorBase<MyModel>
{
    public MyModelValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage(RequiredConstants.Message);
                
        RuleFor(c => c.Name)
            .MaximumLength(20);
    }
}