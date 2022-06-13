using Airslip.Common.Services.Consent.Data;
using Airslip.Common.Types.Hateoas;
using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;

namespace Airslip.Common.Services.Consent.Models
{
    public record AccountResponse : AccountModel, ILinkResourceBase, ISuccess
    {
        public T AddHateoasLinks<T>(string baseUri, params string[] identifiers) where T : class
        {
            Links = new List<Link>
            {
                new(EndPoints.GetAccount(baseUri, Id!), "self", "GET"),
                new(EndPoints.GetTransactions(baseUri, Id!), "get-transactions", "GET")
            };

            return (this as T)!;
        }

        public T AddChildHateoasLinks<T>(T t, string baseUri) where T : class
        {
            return (this as T)!;
        }

        public List<Link> Links { get; set; } = new();
    }
}