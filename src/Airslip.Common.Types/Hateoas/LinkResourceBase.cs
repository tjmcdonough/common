using System.Collections.Generic;

namespace Airslip.Common.Types.Hateoas
{
    public abstract class LinkResourceBase : ILinkResourceBase
    {
        public List<Link> Links { get; set; } = new();

        public virtual T AddHateoasLinks<T>(string baseUri, params string[] identifiers) where T : class
        {
            return (this as T)!;
        }

        public virtual T AddChildHateoasLinks<T>(T t, string baseUri) where T : class
        {
            return (this as T)!;
        }
    }
}