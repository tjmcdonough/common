using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using JetBrains.Annotations;
using System;

namespace Airslip.Common.Services.Consent.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class IncomingTransactionModel : IModelWithOwnership, IFromDataSource
    {
        public string? Id { get; set; }
        public EntityStatus EntityStatus { get; set; }
        public string BankTransactionId { get; set; } = string.Empty;
        public string? TransactionHash { get; set; }
        public string? UserId { get; set; }
        public string AccountId { get; set; } = string.Empty;
        public string BankId { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public long? AuthorisedDate { get; set; }
        public long CapturedDate { get; set; }
        public decimal Amount { get; set; }
        public string? CurrencyCode { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? AddressLine { get; set; }
        public string? LastCardDigits { get; set; }
        public string? IsoFamilyCode { get; set; }
        public string? ProprietaryCode { get; set; }
        public string? TransactionIdentifier { get; set; }
        public string? Reference { get; set; }
        public string? EntityId { get; set; }
        public AirslipUserType AirslipUserType { get; set; }
        public DataSources DataSource { get; set; } = DataSources.Unknown;
        public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
    }
}