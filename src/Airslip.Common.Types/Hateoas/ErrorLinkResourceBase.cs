using System.Collections.Generic;

namespace Airslip.Common.Types.Hateoas
{
    public abstract class ErrorLinkResourceBase
    {
        public List<Link> Links { get; set; } = new(2);

        public virtual T AddHateoasLinks<T>(string baseUri, string? selfRelativeEndpoint, string? nextRelativeEndpoint)
            where T : class
        {
            if (selfRelativeEndpoint != null)
                Links.Add(new Link($"{baseUri}/{selfRelativeEndpoint}", "self", "GET"));

            if (nextRelativeEndpoint != null)
                Links.Add(new Link($"{baseUri}/{nextRelativeEndpoint}", "next", "GET"));

            return (this as T)!;
        }
    }
}