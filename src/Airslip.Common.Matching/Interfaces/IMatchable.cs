using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Interfaces
{
    public interface IMatchable : IEntityWithId
    {
        long TimeStamp { get; }
        List<MatchMetadata> Metadata { get; }
    }
}