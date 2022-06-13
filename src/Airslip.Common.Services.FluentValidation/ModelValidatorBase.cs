using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using FluentValidation;
using FluentValidation.Results;
using System.Threading.Tasks;

namespace Airslip.Common.Services.FluentValidation
{
    public abstract class ModelValidatorBase<TType> : AbstractValidator<TType>, IModelValidator<TType> 
        where TType : class, IModel
    {
        public async Task<ValidationResultModel> ValidateAdd(TType model)
        {
            ValidationResult validationResult = await ValidateAsync(model);

            ValidationResultModel result = new();
            validationResult.Errors
                .ForEach(o => result.AddMessage(o.PropertyName, o.ErrorMessage));

            return await Task.FromResult(result);
        }

        public Task<ValidationResultModel> ValidateUpdate(TType model) => ValidateAdd(model);
    }
}