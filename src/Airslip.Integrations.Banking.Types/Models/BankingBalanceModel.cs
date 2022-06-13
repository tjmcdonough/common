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
public record BankingBalanceModel : IModelWithOwnership, IFromDataSource
{
    public string? Id { get; set; }
    public EntityStatus EntityStatus { get; set; }
    public string? UserId { get; set; }
    public string? EntityId { get; set; }
    public AirslipUserType AirslipUserType { get; set; }
    
    public string AccountId { get; set; } = string.Empty;
    public BankingBalanceStatus BalanceStatus { get; set; }
    public long Balance { get; set; }
    public string? Currency { get; set; }
    
    public string BankingAccountId { get; set; } = string.Empty;
    public DataSources DataSource { get; set; }
    public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
}