using Airslip.Common.Types.Interfaces;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Consent.Interfaces
{
    public interface IBankService
    {
        Task<IResponse> GetBanks(string countryCode, string? name);
    }
}