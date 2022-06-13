using Airslip.Common.Matching.Data;
using Airslip.Common.Matching.Enum;

namespace Airslip.Common.Matching.Configuration
{
    public class MatchRoute
    {
        public Direction Direction { get; set; } = Direction.Request;
        public MatchTypes MatchType { get; set; } = MatchTypes.Unknown;
        public string RouteTo { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
    }
}