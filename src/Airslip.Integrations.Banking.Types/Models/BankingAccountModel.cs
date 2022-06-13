using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using Airslip.Integrations.Banking.Types.Enums;
using JetBrains.Annotations;
using System;

namespace Airslip.Integrations.Banking.Types.Models;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public record BankingAccountModel : IModelWithOwnership, IFromDataSource
{
    public string? Id { get; set; }
    public EntityStatus EntityStatus { get; set; }
    public string? UserId { get; set; }
    public string? EntityId { get; set; }
    public AirslipUserType AirslipUserType { get; set; }
    
    public string AccountId { get; set; } = string.Empty;
    public string? LastCardDigits { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public BankingUsageTypes UsageType { get; set; } = BankingUsageTypes.UNKNOWN;
    public BankingAccountTypes AccountType { get; set; } = BankingAccountTypes.UNKNOWN;
    public string? SortCode { get; set; }
    public string? AccountNumber { get; set; }
    public string? PANNumber { get; set; }
    public string? Iban { get; set; }
    public string? BIC { get; set; }

    public string BankingBankId { get; set; } = string.Empty;
    public BankingAccountStatus AccountStatus { get; set; } = BankingAccountStatus.Active;
    public DataSources DataSource { get; set; }
    public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
}