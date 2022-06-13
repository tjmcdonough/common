using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;

namespace Airslip.Common.Deletion.Models;

public record DeleteRequest(
    string EntityId,
    AirslipUserType AirslipUserType,
    string UserId) : IModel
{
    // These aren't used...
    public string? Id { get; set; }
    public EntityStatus EntityStatus { get; set; }
}