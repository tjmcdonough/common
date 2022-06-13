using Airslip.Common.Auth.Data;
using Microsoft.AspNetCore.Authentication;

namespace Airslip.Common.Auth.AspNetCore.Schemes
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string ApiKeyScheme = AirslipSchemeOptions.ApiKeyScheme;
        public const string ApiKeyHeaderField = AirslipSchemeOptions.ApiKeyHeaderField;
        
        public string Environment
        {
            get => AirslipSchemeOptions.ThisEnvironment;
            set => AirslipSchemeOptions.ThisEnvironment = value;
        }
    }
}