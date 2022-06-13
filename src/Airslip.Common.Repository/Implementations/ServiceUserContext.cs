using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Repository.Implementations;

public class ServiceUserContext : IUserContext
{
    public string? UserId => null;
    public string? EntityId => null;
    public AirslipUserType? AirslipUserType => Common.Types.Enums.AirslipUserType.Merchant;
}