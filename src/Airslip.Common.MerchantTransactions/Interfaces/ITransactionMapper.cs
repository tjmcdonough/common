using Airslip.Common.MerchantTransactions.Generated;
using Airslip.Common.Types.Enums;

namespace Airslip.Common.MerchantTransactions.Interfaces;

public interface ITransactionMapper<in TSource> 
    where TSource : class
{
    TransactionDetails Create(TSource transaction, PosProviders provider);
}