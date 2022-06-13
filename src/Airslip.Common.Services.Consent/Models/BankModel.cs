using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Services.Consent.Models
{
    public record BankModel : IModel, IFromDataSource
    {
        public string? Id { get; set; }
        public string TradingName { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string EnvironmentType { get; set; } = string.Empty;
        public List<string> CountryCodes { get; set; } = new();
        public EntityStatus EntityStatus { get; set; }
        public DataSources DataSource { get; set; } = DataSources.Unknown;
        public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
    }
}