using System.Collections.Generic;

namespace Airslip.Common.Types.Failures;

public class ConflictResponse : ErrorResponse
{
    public ConflictResponse()
    {
        
    }
    
    public ConflictResponse(string attribute, string value, string message)
        : base(
            "RESOURCE_EXISTS",
            message,
            new Dictionary<string, object>
            {
                { "Attribute", attribute },
                { "Value", value },
                { "Validation", message }
            })
    {
    }
        
    public ConflictResponse(string attribute, string value)
        : base(
            "RESOURCE_EXISTS",
            $"{value} already exists",
            new Dictionary<string, object>
            {
                { "Attribute", attribute },
                { "Value", value }
            })
    {
    }
}