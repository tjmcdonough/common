using Airslip.Common.Matching.Enum;
using Airslip.Common.MerchantTransactions.Generated;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Matching.Response
{
    public class TransactionResponse : ISuccess
    {
        public TransactionResponseStatus Status { get; init; }
        public string? ErrorMessage { get; init; }
        public TransactionDetails? Transaction { get; set; }
    }
}