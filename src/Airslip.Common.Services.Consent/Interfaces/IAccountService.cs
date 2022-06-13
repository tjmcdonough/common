using Airslip.Common.Types.Interfaces;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Consent.Interfaces
{
    public interface IAccountService
    {
        Task<IResponse> GetAccounts();
        Task<IResponse> GetAccount(string accountId);
    }
}