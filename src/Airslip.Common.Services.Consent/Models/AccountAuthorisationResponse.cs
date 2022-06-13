using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Services.Consent.Models
{
    public record AccountAuthorisationResponse(string AuthorisationUrl) : ISuccess;
}