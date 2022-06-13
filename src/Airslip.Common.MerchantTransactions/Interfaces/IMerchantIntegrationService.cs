using Airslip.Common.MerchantTransactions.Generated;
using Airslip.Common.Types.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.MerchantTransactions.Interfaces
{
    public interface IMerchantIntegrationService<TSource>
        where TSource : class
    {
        Task<ICollection<TrackingDetails>> SendBulk(
            IEnumerable<TSource> transactions,
            string accountId,
            string entityId,
            AirslipUserType airslipUserType,
            string userId,
            PosProviders provider);

        Task<TrackingDetails> Send(TSource transaction,
            string accountId,
            string entityId,
            AirslipUserType airslipUserType,
            string userId,
            PosProviders provider);

        Task<ICollection<TrackingDetails>> SendBulk(
            IEnumerable<TSource> transactions,
            string airslipApiKey,
            PosProviders provider);

        Task<TrackingDetails> Send(
            TSource transaction, 
            string airslipApiKey, 
            PosProviders provider);
    }
}