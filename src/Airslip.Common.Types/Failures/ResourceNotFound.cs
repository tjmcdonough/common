using System.Collections.Generic;

namespace Airslip.Common.Types.Failures;

public class ResourceNotFound : ErrorResponse
{
    public ResourceNotFound()
    {
            
    }
        
    public ResourceNotFound(string resource, string validation)
        : base(
            "RESOURCE_NOT_FOUND",
            "The resource could not be found. {validation}.",
            new Dictionary<string, object>
            {
                { "resource", resource },
                { "validation", validation }
            }
        )
    {
    }

    public ResourceNotFound(string resource, string property, string value, string validation)
        : base(
            "RESOURCE_NOT_FOUND",
            "The resource {resource} for {property} with the value {value} could not be found. {validation}.",
            new Dictionary<string, object>
            {
                { "resource", resource },
                { "property", property },
                { "value", value },
                { "validation", validation }
            }
        )
    {
    }
}