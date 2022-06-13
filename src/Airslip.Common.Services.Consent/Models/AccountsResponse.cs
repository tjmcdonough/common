using Airslip.Common.Services.Consent.Data;
using Airslip.Common.Types.Hateoas;
using Airslip.Common.Types.Interfaces;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Airslip.Common.Services.Consent.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AccountsResponse : LinkResourceBase, ISuccess
    {
        public List<AccountResponse> Accounts { get; set; }

        public AccountsResponse(List<AccountResponse> accounts)
        {
            Accounts = accounts;
        }

        public override T AddHateoasLinks<T>(string baseUri, params string[] identifiers)
        {
            Links = new List<Link>
            {
                new(EndPoints.GetAccounts(baseUri), "self", "GET")
            };

            return (this as T)!;
        }

        public override T AddChildHateoasLinks<T>(T t, string baseUri)
        {
            AccountsResponse? accountsResponse = t as AccountsResponse;

            foreach (var account in accountsResponse!.Accounts)
            {
                account.AddHateoasLinks<AccountsResponse>(baseUri);
            }

            return (this as T)!;
        }
    }
}