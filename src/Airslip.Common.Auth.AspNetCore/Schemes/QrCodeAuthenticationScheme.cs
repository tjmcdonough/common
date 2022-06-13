using Airslip.Common.Auth.Data;
using Microsoft.AspNetCore.Authentication;

namespace Airslip.Common.Auth.AspNetCore.Schemes
{
    public class QrCodeAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string QrCodeAuthScheme = AirslipSchemeOptions.QrCodeAuthScheme;
        
        public string Environment
        {
            get => AirslipSchemeOptions.ThisEnvironment;
            set => AirslipSchemeOptions.ThisEnvironment = value;
        }
    }
}