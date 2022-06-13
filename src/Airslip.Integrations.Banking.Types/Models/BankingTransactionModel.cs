using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using Airslip.Integrations.Banking.Types.Enums;
using System;

namespace Airslip.Integrations.Banking.Types.Models;

public record BankingTransactionModel : IModelWithOwnership, IFromDataSource
{
    public string? Id { get; set; } 
    public EntityStatus EntityStatus { get; set; } = EntityStatus.Active;
    public string? UserId { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public AirslipUserType AirslipUserType { get; set; }
    
    public string AccountId { get; set; } = string.Empty;
    public long? AuthorisedDate { get; set; }
    public long CapturedDate { get; set; }
    public long Amount { get; set; }
    public string? CurrencyCode { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? LastCardDigits { get; set; }
    public BankingIsoFamilyCodes? IsoFamilyCode { get; set; }
    public string? ProprietaryCode { get; set; }
    
    public string BankingAccountId { get; set; } = string.Empty;
    public string BankingBankId { get; set; } = string.Empty;
    public string BankingSyncRequestId { get; set; } = string.Empty;
    public DataSources DataSource { get; set; }
    public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
}