namespace Airslip.Common.Repository.Types.Models;

public class EntitySearchPagingModel
{
    public int? TotalRecords { get; init; }
    public int? RecordsPerPage { get; init; }
    public int Page { get; set; }
    public EntitySearchQueryModel? Next { get; set; }
}