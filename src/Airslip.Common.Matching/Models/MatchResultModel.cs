using Airslip.Common.Matching.Data;
using Airslip.Common.Matching.Enum;
using Airslip.Common.Types;
using Airslip.Common.Types.Enums;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Models
{
    public record MatchResultModel(MatchTypes MatchType,
        string TransactionTrackingId,
        string MatchTrackingId,
        string? UserId,
        MatchLikelihood MatchLikelihood,
        string? EntityId = null,
        AirslipUserType AirslipUserType = AirslipUserType.Standard)
    {
        public List<MatchMetadata> Metadata { get; } = new();
    }
    
    public record AttemptMatchRequest(MatchPerspective Perspective, string TrackingId);
}