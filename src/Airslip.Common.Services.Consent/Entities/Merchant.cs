using Airslip.Common.Repository.Types.Entities;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities;
using Airslip.Common.Utilities.Extensions;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Services.Consent.Entities
{
    public record Merchant(string Name, MerchantTypes Type) : IEntity, IFromDataSource
    {
        public string Id { get; set; } = CommonFunctions.GetId();
        public List<string> BankStatementDescriptions { get; private set; } = new List<string>();
        public IEnumerable<string>? BankStatementRegex { get; init; }
        public string? CategoryCode { get; init; }
        public string? Mid { get; init; }
        public string? StreetAddress { get; init; }
        public string? City { get; init; }
        public string? County { get; init; }
        public string? Postcode { get; init; }
        public string? CountryCode { get; init; }
        public string? WebsiteUrl { get; init; }
        public string? Email { get; init; }
        public string? ContactNumber { get; init; }
        public string? CompanyNumber { get; init; }
        public string? VatNumber { get; init; }
        public string? Latitude { get; init; }
        public string? Longitude { get; init; }
        public Dictionary<string, object>? Metadata { get; init; }
        public long CreatedTimestamp { get; private set; } = DateTimeExtensions.GetTimestamp();
        public BasicAuditInformation? AuditInformation { get; set; }
        public EntityStatus EntityStatus { get; set; }
        public DataSources DataSource { get; set; } = DataSources.Unknown;
        public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
    }
}