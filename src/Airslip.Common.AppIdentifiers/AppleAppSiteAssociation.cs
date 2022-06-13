using Newtonsoft.Json;
using System.Collections.Generic;

namespace Airslip.Common.AppIdentifiers
{
    public class AppleAppSiteAssociation
    {
        public Applinks? Applinks { get; set; }
        public Webcredentials? Webcredentials { get; set; }
        public Appclips? appclips { get; set; }
    }
    
    public class Component
    {
        [JsonProperty("/")]
        public string? slash { get; set; }
        public string? comment { get; set; }
    }

    public class Detail
    {
        public List<string>? appIDs { get; set; }
        public List<Component>? components { get; set; }
    }

    public class Applinks
    {
        public List<Detail>? details { get; set; }
    }

    public class Webcredentials
    {
        public List<string>? apps { get; set; }
    }

    public class Appclips
    {
        public List<string>? apps { get; set; }
    }
    
    public class Target
    {
        public Target(string @namespace, string packageName, IEnumerable<string> sha256CertFingerprints)
        {
            Namespace = @namespace;
            PackageName = packageName;
            Sha256CertFingerprints = sha256CertFingerprints;
        }

        public string Namespace { get; }
        public string PackageName { get; }
        public IEnumerable<string> Sha256CertFingerprints { get; }
    }
    
    public class AssetLink
    {
        public IEnumerable<string> Relation { get; }
        public Target Target { get; }
        
        public AssetLink(string @namespace, string packageName, IEnumerable<string> relation, IEnumerable<string> sha256CertFingerprints)
        {
            Relation = relation;
            Target = new Target(@namespace, packageName, sha256CertFingerprints);
        }
    }
}