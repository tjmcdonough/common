using System.Collections.Generic;

namespace Airslip.Common.Repository.Types.Models;

public record EntitySearchQueryModel(int Page, int RecordsPerPage, 
    List<EntitySearchSortModel> Sort, 
    EntitySearchModel? Search);