using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using Airslip.Integrations.Banking.Types.Enums;
using System;

namespace Airslip.Integrations.Banking.Types.Models;

public class BankingSyncRequestModel : IModelWithOwnership, IFromDataSource
{
    public string? Id { get; set; }
    public EntityStatus EntityStatus { get; set; }
    public string? UserId { get; set; }
    public string? EntityId { get; set; }
    public AirslipUserType AirslipUserType { get; set; }
    
    public string BankingAccountId { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public BankingUsageTypes UsageType { get; set; }
    public BankingAccountTypes AccountType { get; set; }
    public BankingSyncStatus SyncStatus { get; set; }
    public int RecordCount { get; set; }
    public string? TracingId { get; set; }
    public DataSources DataSource { get; set; }
    public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
}