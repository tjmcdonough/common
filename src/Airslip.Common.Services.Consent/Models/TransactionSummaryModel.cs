using Airslip.Common.Matching.Data;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Types.Hateoas;
using JetBrains.Annotations;

namespace Airslip.Common.Services.Consent.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class TransactionSummaryModel : LinkResourceBase, IMerchantDetails
    {
        public string? Id { get; set; } = string.Empty;
        public EntityStatus EntityStatus { get; set; }
        public MatchTypes MatchType { get; set; } = MatchTypes.Unknown;
        public string OriginalTrackingId { get; set; } = string.Empty;
        public string? MatchedTrackingId { get; set; }
        public long? AuthorisedTimeStamp { get; set; }
        public long? CapturedTimeStamp { get; set; }
        public long TimeStamp { get; set; }
        public string? Amount { get; set; }
        public string? CurrencyCode { get; set; }
        public string? Description { get; set; }
        public MerchantSummaryModel MerchantDetails { get; set; } = new();
        public string? AccountId { get; set; }
    }
}
