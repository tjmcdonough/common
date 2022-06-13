using System.Collections.Generic;

namespace Airslip.Common.Types.Hateoas
{
    public interface ILinkResourceBase
    {
        List<Link> Links { get; set; }

        T AddHateoasLinks<T>(string baseUri, params string[] identifiers) where T : class;

        T AddChildHateoasLinks<T>(T t, string baseUri) where T : class;
    }
}