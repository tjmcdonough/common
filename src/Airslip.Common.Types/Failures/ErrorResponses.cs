using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Types.Failures;

public class ErrorResponses : IFail
{
    public static readonly ErrorResponses Empty = new();

    private ErrorResponses()
    {
        Errors = new List<ErrorResponse>();
    }

    public ICollection<ErrorResponse> Errors { get; }

    public ErrorResponses(ICollection<ErrorResponse> errors)
    {
        Errors = errors;
    }

    public string ErrorCode => Errors.FirstOrDefault()?.ErrorCode ?? "UNHANDLED_EXCEPTION";
}