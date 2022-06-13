using Airslip.Common.Types;
using Airslip.Common.Types.Failures;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Airslip.Common.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            ObjectResult errorResponse;

            try
            {
                await _next(context);
                return;
            }
            catch (ValidationException exception)
            {
                string[] errorCodes = exception.Errors.Select(e => e.ErrorCode).ToArray();
                string compressed = string.Join(",", errorCodes);

                _logger.Warning("ErrorMessages {Compressed}", compressed);

                IFail[] errors = exception.Errors
                    .Select(error => new ErrorResponse(error.ErrorCode, error.ErrorMessage, new RouteValueDictionary(error.CustomState)!))
                    .Cast<IFail>()
                    .ToArray();

                errorResponse = new ObjectResult(errors)
                {
                    StatusCode = (int) HttpStatusCode.BadRequest
                };
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "An unhandled error occurred");

                ErrorResponses errorResponses = ErrorResponses.Empty;
                
                errorResponses.Errors.Add(new ErrorResponse(
                    "UNHANDLED_ERROR",
                    exception.Message,
                    new Dictionary<string, object> {{"timestamp", DateTimeOffset.UtcNow.ToString("s")}}));
                
                errorResponse = new ObjectResult(errorResponses)
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError
                };
            }

            context.Response.ContentType = Json.MediaType;
            context.Response.StatusCode = errorResponse.StatusCode ?? (int) HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(Json
                .Serialize(errorResponse.Value!, Casing.CAMEL_CASE, Formatting.Indented, NullValueHandling.Ignore));
        }
    }
}