using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.AppIdentifiers
{
    public class AppleAppIdentificationService : IAppleAppIdentificationService
    {
        private readonly SettingCollection<AppleAppIdentifierSetting> _appleAppIdentifierSettings;
        private readonly AndroidAppIdentifierSettings _androidAppIdentifierSettings;

        public AppleAppIdentificationService(
            IOptions<SettingCollection<AppleAppIdentifierSetting>> appIdentifierOptions,
            IOptions<AndroidAppIdentifierSettings> packageOptions)
        {
            _appleAppIdentifierSettings = appIdentifierOptions.Value;
            _androidAppIdentifierSettings = packageOptions.Value;
        }

        public AppleAppSiteAssociation GetAppSiteAssociation()
        {
            List<string> appIds = _appleAppIdentifierSettings
                .Settings!.Values.Select(o => o.AppID).Distinct().ToList();

            IEnumerable<Component> components = _appleAppIdentifierSettings
                .Settings!
                .Values
                .Select(o =>
                {
                    string path = BuildDeepLinkingPath(o);
                    return new Component()
                    {
                        comment = "Matches any URL whose path starts with " + path,
                        slash = path
                    };
                });
            
            return new AppleAppSiteAssociation
            {
                Applinks = new Applinks
                {
                    details = new List<Detail>
                    {
                        new()
                        {
                            appIDs = appIds,
                            components = components.ToList()
                        }
                    }
                },
                Webcredentials = new Webcredentials
                {
                    apps = appIds
                },
                appclips = new Appclips
                {
                    apps = appIds
                }
            };
        }

        private string BuildDeepLinkingPath(AppleAppIdentifierSetting setting)
        {
            return string.IsNullOrWhiteSpace(setting.UriSuffix)
                ? $"/{setting.Version}/{setting.Endpoint}/*"
                : $"/{setting.UriSuffix}/{setting.Version}/{setting.Endpoint}/*";
        }

        public IEnumerable<AssetLink> GetAssetLinks()
        {
            PackageConfiguration androidPackageSettings = _androidAppIdentifierSettings.Android;

            return new List<AssetLink>
            {
                new(androidPackageSettings.Namespace, androidPackageSettings.PackageName,
                    androidPackageSettings.Relation, androidPackageSettings.Sha256CertFingerprints)
            };
        }
    }
}