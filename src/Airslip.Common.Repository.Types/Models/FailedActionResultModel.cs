using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Repository.Types.Models;

public record FailedActionResultModel<TModel>(
        string ErrorCode,
        ResultType ResultType,
        TModel? CurrentVersion = null,
        TModel? PreviousVersion = null,
        ValidationResultModel? ValidationResult = null)
    : RepositoryActionResultModel<TModel>(ResultType, CurrentVersion, PreviousVersion, ValidationResult), IFail
    where TModel : class, IModel;