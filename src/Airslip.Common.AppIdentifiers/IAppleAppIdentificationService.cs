using System.Collections.Generic;

namespace Airslip.Common.AppIdentifiers
{
    public interface IAppleAppIdentificationService
    {
        AppleAppSiteAssociation GetAppSiteAssociation();
        IEnumerable<AssetLink> GetAssetLinks();
    }
}