using System.Collections.Generic;

namespace Airslip.Common.Repository.Types.Models;

public record EntitySearchModel(List<SearchFilterModel> Items, string LinkOperator = "and");