using System.Collections.Generic;

namespace Airslip.Common.AppIdentifiers
{
    public class AndroidAppIdentifierSettings
    {
        public PackageConfiguration Android { get; set; } = new();
    }

    public class PackageConfiguration
    {
        public string Namespace { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public ICollection<string> Sha256CertFingerprints { get; set; } = new List<string>(1);
        public ICollection<string> Relation { get; set; } = new List<string>(1);
    }
}