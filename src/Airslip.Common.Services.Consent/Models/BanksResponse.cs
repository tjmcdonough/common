using Airslip.Common.Types.Hateoas;
using Airslip.Common.Types.Interfaces;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Airslip.Common.Services.Consent.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class BanksResponse : LinkResourceBase, ISuccess
    {
        public ICollection<BankResponse> Banks { get; }

        public BanksResponse(ICollection<BankResponse> banks)
        {
            Banks = banks;
        }
    }
}