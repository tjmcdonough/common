using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;

namespace Airslip.Common.Types.Responses
{
    public class ListResult<T> : ISuccess
    {
        /// <summary>
        ///     The list of data items in the current page.
        /// </summary>
        public List<T> Results { get; init; } = new();
    }
}