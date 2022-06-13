using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Matching.Interfaces
{
    public interface IDeDupeService<TType>
    {
        Task<List<TType>> DeDupeRecords(List<TType> records);
    }
}