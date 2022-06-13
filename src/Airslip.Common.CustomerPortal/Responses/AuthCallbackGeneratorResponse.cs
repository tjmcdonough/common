using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.CustomerPortal.Responses
{
    public record AuthCallbackGeneratorResponse(string CallBackUrl) : ISuccess;
}