using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using System;
using System.Collections.Generic;

namespace Airslip.Integrations.Banking.Types.Models;

public record BankingBankModel : IModel, IFromDataSource
{
    public string? Id { get; set; }
    public EntityStatus EntityStatus { get; set; }
    public string TradingName { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public EnvironmentType EnvironmentType { get; set; }
    public ICollection<string> CountryCodes { get; set; } = new List<string>();
    public DataSources DataSource { get; set; }
    public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
    public int Priority { get; set; }
    public string? Icon { get; set; }
    public string? Logo { get; set; }
}