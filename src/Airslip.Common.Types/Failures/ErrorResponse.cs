using Airslip.Common.Types.Hateoas;
using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Types.Failures;

public class ErrorResponse : ErrorLinkResourceBase, IFail
{
    public string ErrorCode { get; set; } = string.Empty;
    public string? Message { get; set; }
    public IDictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

    public ErrorResponse()
    {
            
    }

    public ErrorResponse(string errorCode, string? message = null, IDictionary<string, object>? metadata = null)
    {
        Message = metadata == null
            ? message
            : metadata.Aggregate(message,
                (current, pair) => current?.Replace($"{{{pair.Key}}}", $"{pair.Value}"));
        ErrorCode = errorCode;
        Metadata = metadata ?? new Dictionary<string, object>();
    }

    public ErrorResponse Add(string key, object value)
    {
        Metadata.Add(key, value);
        return this;
    }
}