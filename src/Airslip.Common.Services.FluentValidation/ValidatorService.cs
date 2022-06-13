using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Types;
using Airslip.Common.Types.Failures;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Types.Validator;
using Airslip.Common.Utilities.Extensions;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Services.FluentValidation;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class ValidatorService
{
    public static async Task<IResponse> Validate<T>(
        this AbstractValidator<T> validator,
        T request) where T : class
    {
        ValidationResult? validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsValid)
            return Success.Instance;

        ErrorResponse[] errors = validationResult.Errors
            .Select(error =>
                new ErrorResponse(
                    error.ErrorCode,
                    error.ErrorMessage,
                    error.CustomState.ToDictionary<object>()))
            .ToArray();

        return new ErrorResponses(errors);
    }
        
    public static async Task<IResponse> ValidateAdd<T>(
        this IModelValidator<T> validator,
        T request) where T : class, IModel
    {
        return await _validate(validator.ValidateAdd, request);
    }
        
    public static async Task<IResponse> ValidateUpdate<T>(
        this IModelValidator<T> validator,
        T request) where T : class, IModel
    {
        return await _validate(validator.ValidateUpdate, request);
    }

    private static async Task<IResponse> _validate<T>(Func<T, Task<ValidationResultModel>> validationFunction, T model)
    {
        ValidationResultModel validationResult = await validationFunction(model);

        if (validationResult.IsValid)
            return Success.Instance;

        ErrorResponse[] errors = validationResult.Results
            .Select(error =>
                new ErrorResponse(
                    error.ErrorCode ?? InvalidConstants.UnhandledError,
                    error.Message))
            .ToArray();

        return new ErrorResponses(errors);
    }
}