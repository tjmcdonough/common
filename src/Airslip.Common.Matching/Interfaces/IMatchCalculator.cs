using Airslip.Common.Matching.Enum;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Matching.Interfaces
{
    public interface IMatchCalculator
    {
        IResponse CalculateMatchLikelihood(MatchPerspective matchPerspective, IMatchable source, IMatchable with);
    }
}