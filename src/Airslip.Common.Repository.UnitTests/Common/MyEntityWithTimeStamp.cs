using Airslip.Common.Repository.Types.Entities;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;

namespace Airslip.Common.Repository.UnitTests.Common;

public class MyEntityWithTimeStamp : IEntityWithOwnership, IEntityWithTimeStamp
{
    public string Id { get; set; } = string.Empty;
    public BasicAuditInformation? AuditInformation { get; set; }
    public EntityStatus EntityStatus { get; set; }
    public string? UserId { get; set; }
    public string? EntityId { get; set; }
    public AirslipUserType AirslipUserType { get; set; }
    public string Name { get; set; } = string.Empty;
    public long TimeStamp { get; set; }
}