using Airslip.Common.Matching.Data;
using Airslip.Common.Repository.Types.Entities;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities;
using Airslip.Common.Utilities.Extensions;
using System;

namespace Airslip.Common.Services.Consent.Entities
{
    public class Transaction : IFromDataSource, IEntityWithOwnership
    {
        public string Id { get; set; } = CommonFunctions.GetId();
        public BasicAuditInformation? AuditInformation { get; set; }
        public EntityStatus EntityStatus { get; set; }
        public string? UserId { get; set; }
        public string? EntityId { get; set; }
        public AirslipUserType AirslipUserType { get; set; }
        public MatchTypes MatchType { get; set; } = MatchTypes.Unknown;
        public string OriginalTrackingId { get; set; } = string.Empty;
        public string MatchedTrackingId { get; set; } = string.Empty;
        public long? AuthorisedTimeStamp { get; set; }
        public long? CapturedTimeStamp { get; set; }
        public long? Amount { get; set; }
        public string? CurrencyCode { get; set; }
        public string? Description { get; set; }
        public TransactionBank BankDetails { get; set; } = new();
        public TransactionMerchant Merchant { get; set; } = new();
        public TransactionMetadata Metadata { get; set; } = new();
        public DataSources DataSource { get; set; } = DataSources.Unknown;
        public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
    }
}