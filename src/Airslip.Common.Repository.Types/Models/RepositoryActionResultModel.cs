using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Repository.Types.Models;

public abstract record RepositoryActionResultModel<TModel>
(ResultType ResultType, TModel? CurrentVersion = null,
    TModel? PreviousVersion = null,
    ValidationResultModel? ValidationResult = null) : IResponse
    where TModel : class, IModel;