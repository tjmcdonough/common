using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Matching.Data
{
    public record MatchRejected(string ErrorCode) : IFail;
}