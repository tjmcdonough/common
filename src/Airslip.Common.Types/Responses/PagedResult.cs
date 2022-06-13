using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;

namespace Airslip.Common.Types.Responses
{
    public class PagedResult<T> : ISuccess
    {
        /// <summary>
        ///     The start pointer for the current results
        /// </summary>
        public int? SliceStart { get; init; }
        
        /// <summary>
        ///     The current page size.
        /// </summary>
        public int? SliceEnd { get; init; }

        /// <summary>
        ///     The number of results in the result set.
        /// </summary>
        public int? TotalRecords { get; init; }

        /// <summary>
        ///     The list of data items in the current page.
        /// </summary>
        public List<T> Results { get; init; } = new();
    }
}