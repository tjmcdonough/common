using Airslip.Common.Auth.Data;
using Microsoft.AspNetCore.Authentication;

namespace Airslip.Common.Auth.AspNetCore.Schemes
{
    public class CookieAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string CookieAuthScheme = CookieSchemeOptions.CookieAuthScheme;
    
        public string Environment
        {
            get => AirslipSchemeOptions.ThisEnvironment;
            set => AirslipSchemeOptions.ThisEnvironment = value;
        }
    }
    
    public static class CookieSchemeOptions
    {
        public const string CookieAuthScheme = "CookieAuthScheme";
        
        public const string CookieTokenField = "tf";
        public const string CookieEncryptField = "tfe";
        public const string CookieCyclesField = "ec";
        public const string CookieTimestampField = "ts";
    }
}