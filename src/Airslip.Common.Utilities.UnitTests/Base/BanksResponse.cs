using System.Collections.Generic;

namespace Airslip.Common.Utilities.UnitTests.Base
{
    public class BanksResponse
    {
        public ICollection<Bank> Banks { get; }

        public BanksResponse(ICollection<Bank> banks)
        {
            Banks = banks;
        }
    }
    
    public class Bank
    {
        public string Name { get; }
        public string LogoName { get; }
        public string LogoUrl { get; }
        public string IconName { get; }
        public string IconUrl { get; }
        public ICollection<BankTypeResponse> BankTypes { get; }

        public Bank(
            string name,
            string logoName,
            string logoUrl,
            string iconName,
            string iconUrl,
            ICollection<BankTypeResponse> bankTypes)
        {
            Name = name;
            LogoName = logoName;
            LogoUrl = logoUrl;
            IconName = iconName;
            IconUrl = iconUrl;
            BankTypes = bankTypes;
        }
    }

    public class BankTypeResponse
    {
        public string Id { get; }
        public string Name { get; }

        public BankTypeResponse(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}