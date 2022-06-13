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
    public record MerchantModel : IModel, IFromDataSource
    {
        public string? Id { get; set; }
        public EntityStatus EntityStatus { get; set; }
        public string Name { get; set; }  = string.Empty;
        public MerchantTypes Type { get; set; }
        public IEnumerable<string> BankStatementDescriptions { get; set;  } = new List<string>();
        public IEnumerable<string>? BankStatementRegex { get; set; }
        public string? CategoryCode { get; set; }
        public string? Mid { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? County { get; set; }
        public string? Postcode { get; set; }
        public string? CountryCode { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? CompanyNumber { get; set; }
        public string? VatNumber { get; set; }
        public IDictionary<string, object>? Metadata { get; set; }
        public DataSources DataSource { get; set; } = DataSources.Unknown;
        public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
    }
}