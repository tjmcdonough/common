using System.Collections.Generic;

namespace Airslip.Common.Types.Failures;

public class ExternalLoginFailed : ErrorResponse
{
    public ExternalLoginFailed()
    {
        
    }
    
    public ExternalLoginFailed(string provider)
        : base(
            "EXTERNAL_LOGIN_FAILED",
            "The provider {provider} rejected login.",
            new Dictionary<string, object>
            {
                { "provider", provider }
            }
        )
    {
    }
}