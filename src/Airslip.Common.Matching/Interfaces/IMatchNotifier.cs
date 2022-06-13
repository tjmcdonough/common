using Airslip.Common.Matching.Models;
using System.Threading.Tasks;

namespace Airslip.Common.Matching.Interfaces
{
    public interface IMatchNotifier
    {
        Task NotifyMatch(MatchResultModel matchResult);
    }
}