using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;

namespace Airslip.Common.Repository.Types.Entities;

public class AdditionalOwner : IOwnership, IEntityWithId
{
    public string Id { get; set; } = string.Empty;
    
    public string? UserId { get; set; }
        
    public string? EntityId { get; set; }
        
    public AirslipUserType AirslipUserType { get; set; }
}