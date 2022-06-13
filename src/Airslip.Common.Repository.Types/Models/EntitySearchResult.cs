using System.Collections.Generic;

namespace Airslip.Common.Repository.Types.Models;

public record EntitySearchResult<TEntity>(List<TEntity> Records, int RecordCount);