using System.Collections.Generic;

namespace Airslip.Common.Types.Failures;

public class InvalidAttribute : ErrorResponse
{
    public InvalidAttribute()
    {
            
    }
        
    public InvalidAttribute(string attribute, string value, string validation)
        : base(
            "INVALID_ATTRIBUTE",
            "The attribute {attribute} with the value {value} is invalid. {validation}.",
            new Dictionary<string, object>
            {
                { "attribute", attribute },
                { "value", value },
                { "validation", validation }
            }
        )
    {
    }
}