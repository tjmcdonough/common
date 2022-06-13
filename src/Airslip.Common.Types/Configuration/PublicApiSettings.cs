using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;

namespace Airslip.Common.Types.Configuration
{
    public class PublicApiSettings : SettingCollection<PublicApiSetting>
    {
        public PublicApiSetting Base { get; set; } = new();
        public PublicApiSetting? MerchantTransactions { get; set; }
        public PublicApiSetting? MerchantDatabase { get; set; }
        public PublicApiSetting? Identity { get; set; }
        public PublicApiSetting? BankTransactions { get; set; }
        public PublicApiSetting? Notifications { get; set; }
        public PublicApiSetting? QrCodeMatching { get; set; }
    }
    
    public class SettingCollection<TType> : ISettingWithDictionary<TType>
    {
        public Dictionary<string, TType>? Settings { get; set; }
    }
}