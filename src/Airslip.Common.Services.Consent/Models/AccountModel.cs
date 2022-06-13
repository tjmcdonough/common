using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using JetBrains.Annotations;
using System;

namespace Airslip.Common.Services.Consent.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public record AccountModel : IModelWithOwnership, IFromDataSource, ISuccess
    {
        public string? Id { get; set; }
        public EntityStatus EntityStatus { get; set; }
        public string AccountId { get; set; } = string.Empty;
        public DataSources DataSource { get; set; } = DataSources.Unknown;
        public string? UserId { get; set; }
        public string? EntityId { get; set; }
        public AirslipUserType AirslipUserType { get; set; }
        public string? LastCardDigits { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string UsageType { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public string? SortCode { get; set; }
        public string? AccountNumber { get; set; }
        public long CreatedTimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
        public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
        public string InstitutionId { get; set; } = string.Empty;
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;
    }
}
    