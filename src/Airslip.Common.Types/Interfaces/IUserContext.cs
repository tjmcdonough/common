using Airslip.Common.Types.Enums;

namespace Airslip.Common.Types.Interfaces
{
    public interface IUserContext
    {
        string? UserId { get; }
        string? EntityId { get; }
        AirslipUserType? AirslipUserType { get; }
    }
}
