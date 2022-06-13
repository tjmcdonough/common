using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Repository.Types.Models;

public record SuccessfulActionResultModel<TModel>(
        TModel? CurrentVersion = null,
        TModel? PreviousVersion = null,
        ValidationResultModel? ValidationResult = null)
    : RepositoryActionResultModel<TModel>(ResultType.Success, CurrentVersion, PreviousVersion, ValidationResult), ISuccess
    where TModel : class, IModel;