using Airslip.Common.Repository.Types.Entities;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using JetBrains.Annotations;
using System;

namespace Airslip.Common.Services.Consent.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Account : IFromDataSource, IEntityWithOwnership
    {
        public string Id { get; set; } = string.Empty;
        public BasicAuditInformation? AuditInformation { get; set; }
        public EntityStatus EntityStatus { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public DataSources DataSource { get; set; } = DataSources.Unknown;
        public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
        public string AccountId { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? EntityId { get; set; }
        public AirslipUserType AirslipUserType { get; set; }
        public string? LastCardDigits { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string UsageType { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public string? SortCode { get; set; }
        public string? AccountNumber { get; set; }
        public string BankId { get; set; } = string.Empty;
        public string InstitutionId { get; set; } = string.Empty;
    }
}