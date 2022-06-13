using Airslip.Common.Types.Interfaces;

namespace Airslip.Integrations.Banking.Types.Responses
{
    public record ConsentResponse(string AuthorisationUrl) : ISuccess;
}