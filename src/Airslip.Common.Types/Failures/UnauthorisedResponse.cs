using System.Collections.Generic;

namespace Airslip.Common.Types.Failures;

public class UnauthorisedResponse : ErrorResponse
{
    public UnauthorisedResponse()
    {
            
    }
        
    public UnauthorisedResponse(string resource, string validation)
        : base(
            "UNAUTHORISED",
            "Access is not allowed to the {resource} resource. {validation}.",
            new Dictionary<string, object>
            {
                { "resource", resource }, 
                { "validation", validation }
            })
    {
    }
}