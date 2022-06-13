using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Types.Hateoas;
using Airslip.Common.Types.Interfaces;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Airslip.Common.Services.Consent.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class TransactionModel : LinkResourceBase, ISuccess, IMerchantDetails
    {
        public string? Id { get; set; }
        public EntityStatus EntityStatus { get; set; }
        public string? AuthorisedDate { get; set; } // May need changing to datetime to localise the date
        public string? AuthorisedTime { get; set; }
        public string? CapturedDate { get; set; } // May need changing to datetime to localise the date 
        public string? CapturedTime { get; set; }
        public string? Amount { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? Description { get; set; }
        public TransactionBankModel? BankDetails { get; set; }
        public TransactionMatchModel? MatchDetails { get; set; }
        public MerchantSummaryModel MerchantDetails { get; set; } = new();
        
        public override T AddHateoasLinks<T>(string baseUri, params string[] identifiers)
        {
            string accountId = BankDetails?.AccountId ?? string.Empty;
            string bankTransactionId = BankDetails?.BankTransactionId ?? string.Empty;
            Links = new List<Link>
            {
                new(Endpoints.GetTransaction(baseUri, accountId,bankTransactionId), "self", "GET"),
                new(Endpoints.GetTransactions(baseUri, accountId), "get-transactions", "GET")
            };

            return (this as T)!;
        }
        
        public static class Endpoints
        {
            public static string GetTransactions(string baseUri, string accountId) 
                => $"{baseUri}/v1/transactions/{accountId}";
            public static string GetTransaction(string baseUri, string accountId, string transactionId) 
                => $"{baseUri}/v1/transactions/{accountId}/{transactionId}";
            public static string GetLogoUrl(string baseUri, string merchantId)
                => $"{baseUri}/merchant/{merchantId}/logo";
            public static string GetIconUrl(string baseUri, string merchantId)
                => $"{baseUri}/merchant/{merchantId}/icon";
        }

    }
}