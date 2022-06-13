using Airslip.Common.Types.Enums;

namespace Airslip.Common.Repository.Types.Interfaces;

public interface IOwnership
{
    string? UserId { get; set; }
        
    string? EntityId { get; set; }
        
    AirslipUserType AirslipUserType { get; set; }
}