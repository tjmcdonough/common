using Airslip.Common.Types.Failures;
using System.Net;

namespace Airslip.Common.Utilities;

public static class ErrorResponseSerializer
{
    public static ErrorResponse TransformToConcreteType(string content, HttpStatusCode httpStatusCode)
    {
        ErrorResponse errorResponse = Json.Deserialize<ErrorResponse>(content);
        
        return httpStatusCode switch
        {
            HttpStatusCode.Conflict => Json.Deserialize<ConflictResponse>(content),
            HttpStatusCode.NotFound => Json.Deserialize<NotFoundResponse>(content),
            _ => errorResponse
        };
    }
}