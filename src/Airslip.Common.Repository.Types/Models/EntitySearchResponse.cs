using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Types.Models;

public class EntitySearchResponse<T> : ISuccess
{
    /// <summary>
    ///     The list of data items in the current slice.
    /// </summary>
    public List<T> Results { get; init; } = new();

    public EntitySearchPagingModel? Paging { get; set; }
}