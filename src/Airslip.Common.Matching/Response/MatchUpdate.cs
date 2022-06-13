using Airslip.Common.Matching.Data;
using Airslip.Common.Types;
using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Response
{
    public record MatchUpdate(MatchTypes MatchType,
        string TransactionTrackingId,
        string MatchedTrackingId,
        string OriginalTrackingId,
        int Score,
        MatchLikelihood MatchLikelihood) : ISuccess
    {
        public string? UserId { get; set; } = string.Empty;
        public List<MatchMetadata> Metadata { get; } = new();
    }
}