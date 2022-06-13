using Airslip.Common.Repository.Types.Interfaces;

namespace Airslip.Common.Services.FluentValidation
{
    public class NullValidator<TType> : ModelValidatorBase<TType> 
        where TType : class, IModel
    {
        
    }
}