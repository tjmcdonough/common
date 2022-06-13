using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Failures;
using Airslip.Common.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Repository.Extensions;

public static class RepositoryExtensions
{
    public static bool IsActive(this EntityStatus entityStatus) 
        => entityStatus == EntityStatus.Active;
        
    public static ErrorResponses ToUnsuccessfulResponse<TModel>(
        this RepositoryActionResultModel<TModel> result, string errorCode = "400") where TModel : class, IModel
    {
        ICollection<ErrorResponse> errors = result.ValidationResult!.Results
            .Select(v => new ErrorResponse(errorCode, v.Message))
            .ToList();

        return new ErrorResponses(errors);
    }

    public static bool CanView<TEntity>(this TEntity entity, IUserContext userService)
        where TEntity: class, IOwnership
    {
        if (userService.AirslipUserType is null) return false;
        if (userService.UserId is null) return false;
        if (entity.AirslipUserType != userService.AirslipUserType) return false;
        if (entity.AirslipUserType == AirslipUserType.Standard && entity.UserId != userService.UserId) return false;
        if (entity.AirslipUserType != AirslipUserType.Standard && entity.EntityId != userService.EntityId) return false;

        return true;
    }

    public static List<ValidationResultMessageModel> CheckApplies<TEntity, TModel>(this RepositoryAction<TEntity, TModel> repositoryAction, 
        IEnumerable<LifecycleStage> appliesTo) 
        where TEntity : class, IEntity 
        where TModel : class, IModel
    {
        List<ValidationResultMessageModel> result = new();
            
        if (!appliesTo.Contains(repositoryAction.LifecycleStage)) 
            result.Add(new ValidationResultMessageModel(nameof(repositoryAction.LifecycleStage), 
                ErrorMessages.LifecycleEventDoesntApply));

        return result;
    }
        
    public static bool CheckApplies(this LifecycleStage lifecycleStage, 
        IEnumerable<LifecycleStage> appliesTo) 
    {
        return appliesTo.Contains(lifecycleStage); 
    }
    
    internal static EntitySearchPagingModel CalculatePagination(this EntitySearchQueryModel entitySearchQueryModel, int recordCount)
    {
        EntitySearchPagingModel result = new()
        {
            TotalRecords = recordCount,
            Page = entitySearchQueryModel.Page,
            RecordsPerPage = entitySearchQueryModel.RecordsPerPage
        };

        int totalPages = (int) Math.Ceiling(recordCount > 0
            ? Convert.ToDouble(recordCount) / Convert.ToDouble(entitySearchQueryModel.RecordsPerPage)
            : 1);

        if (result.Page < totalPages)
        {
            result.Next = entitySearchQueryModel with
            {
                Page = result.Page
            };
        }

        return result;
    }
}