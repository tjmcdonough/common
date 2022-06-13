using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Auth.Functions.Implementations
{
    public class ApiKeyTokenUserService : IUserContext
    {
        private readonly ITokenDecodeService<ApiKeyToken> _tokenDecodeService;

        public ApiKeyTokenUserService(ITokenDecodeService<ApiKeyToken> tokenDecodeService)
        {
            _tokenDecodeService = tokenDecodeService;
        }

        public string? UserId => null;

        public string? EntityId => _tokenDecodeService.GetCurrentToken().EntityId;

        public AirslipUserType? AirslipUserType => _tokenDecodeService.GetCurrentToken().AirslipUserType;
    }
}