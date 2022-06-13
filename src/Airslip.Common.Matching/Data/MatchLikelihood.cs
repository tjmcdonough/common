using Airslip.Common.Matching.Enum;
using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Data
{
    public record MatchLikelihood(MatchTypes MatchSource, MatchPerspective MatchPerspective, string SourceTrackingId,
        string WithTrackingId, int Score, List<MatchMetric> Metrics) : ISuccess;
}