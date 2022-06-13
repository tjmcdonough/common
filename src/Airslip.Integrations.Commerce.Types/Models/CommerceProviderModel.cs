using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using System;

namespace Airslip.Integrations.Commerce.Types.Models;

public record CommerceProviderModel : IModel, IFromDataSource
{
    public string? Id { get; set; }
    public EntityStatus EntityStatus { get; set; } = EntityStatus.Active;
    public EnvironmentType EnvironmentType { get; set; } = EnvironmentType.Live;
    public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
    public DataSources DataSource { get; set; } = DataSources.Unknown;
    public string Name { get; set; } = String.Empty;
    public string Provider { get; set; } = string.Empty;
    public string FriendlyName { get; set; } = string.Empty;
    public string Integration { get; set; } = string.Empty;
    public int Priority { get; set; }
    public AvailabilityType Availability { get; set; } = AvailabilityType.ComingSoon;
}