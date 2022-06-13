using System.Collections.Generic;

namespace Airslip.Common.Types.Failures;

public class HandledError : ErrorResponse
{
    public HandledError()
    {
        
    }
    
    public HandledError(string codeLocation, string message)
        : base(
            "HANDLED_ERROR",
            message,
            new Dictionary<string, object>
            {
                { "resource", codeLocation },
                { "validation", message }
            }
        )
    {
    }
}