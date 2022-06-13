using System.Collections.Generic;

namespace Airslip.Common.Types.Failures;

public class InvalidResource : ErrorResponse
{
    public InvalidResource()
    {
            
    }
        
    public InvalidResource(string resource, string validation)
        : base(
            "INVALID_RESOURCE",
            "The {resource} is invalid. {validation}.",
            new Dictionary<string, object>
            {
                { "resource", resource },
                { "validation", validation }
            }
        )
    {
    }
}