using System.Collections.Generic;

namespace Airslip.Common.Types.Failures;

public class RequiredParameter : ErrorResponse
{
    public RequiredParameter()
    {
            
    }
        
    public RequiredParameter(string parameter, string validation)
        : base(
            "REQUIRED_PARAMETER",
            "The parameter {parameter} is required. {validation}.",
            new Dictionary<string, object>
            {
                { "parameter", parameter },
                { "validation", validation }
            }
        )
    {
    }
}