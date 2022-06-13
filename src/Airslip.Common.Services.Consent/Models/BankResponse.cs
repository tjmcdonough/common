using Airslip.Common.Types.Interfaces;
using JetBrains.Annotations;

namespace Airslip.Common.Services.Consent.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public record BankResponse : BankModel, ISuccess
    {
        
    }
}