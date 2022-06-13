using Airslip.Common.Matching.Data;
using Airslip.Common.Types;
using Airslip.Common.Types.Enums;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Request
{
    public class MatchRequest
    {
        public string? TrackingId { get; set; }
        public long? Timestamp { get; set; }
        public MatchTypes MatchType { get; set; } = MatchTypes.Unknown;
        public string EntityId { get; set; } = string.Empty;
        public AirslipUserType AirslipUserType { get; set; } = AirslipUserType.Merchant;
        public string StoreId { get; set; } = string.Empty;
        public string CheckoutId { get; set; } = string.Empty;
        public string CallbackUrl { get; set; } = string.Empty;
        public List<MatchMetadata> Metadata { get; set; } = new();
    }
}
