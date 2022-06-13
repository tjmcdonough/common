using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Failures;
using Airslip.Common.Types.Hateoas;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Types.Responses;
using Airslip.Common.Utilities.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Airslip.Common.Auth.AspNetCore.Implementations
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ApiControllerBase : ControllerBase
    {
        protected readonly string BaseUri;
        protected readonly UserToken Token;
        protected readonly ILogger _logger;

        public ApiControllerBase(ITokenDecodeService<UserToken> tokenDecodeService, 
            IOptions<PublicApiSettings> publicApiOptions, ILogger logger)
        {
            Token = tokenDecodeService.GetCurrentToken();
            BaseUri = publicApiOptions.Value.Base.ToBaseUri();
            _logger = logger;
        }
        
        protected IActionResult Ok<T>(T response) 
            where T: class, IResponse
        {
            if (response is ILinkResourceBase @base)
            {
                @base.AddHateoasLinks<T>(BaseUri);
                @base.AddChildHateoasLinks(@base, BaseUri);
            }
            return response switch
            {
                DownloadResponse downloadResponse => File(downloadResponse.FileContent, 
                    downloadResponse.MediaType, downloadResponse.FileName), 
                ISuccess _ => new ObjectResult(response) { StatusCode = (int) HttpStatusCode.OK },
                _ => BadRequest(response)
            };
        }

        protected IActionResult Created(IResponse response)
        {
            return response switch
            {
                ISuccess _ => new ObjectResult(response) { StatusCode = (int) HttpStatusCode.Created },
                _ => BadRequest(response)
            };
        }

        protected IActionResult Conflict(IResponse response)
        {
            if (response is not ConflictResponse conflictResponse)
                return BadRequest(response);

            _logger.Warning("Conflict error: {ErrorMessage}", conflictResponse.Message);
            return new ObjectResult(response) { StatusCode = StatusCodes.Status409Conflict };
        }
        
        protected IActionResult NotFound(IResponse response)
        {
            return response switch
            {
                ISuccess _ => new ObjectResult(response) { StatusCode = (int) HttpStatusCode.NotFound },
                _ => BadRequest(response)
            };
        }

        protected IActionResult Unauthorised(IResponse response)
        {
            switch (response)
            {
                case UnauthorisedResponse unauthorisedResponse:
                    _logger.Warning("Unauthorised error: {ErrorMessage}", unauthorisedResponse.Message);
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status401Unauthorized };
                default:
                    return BadRequest(response);
            }
        }

        protected IActionResult BadRequest(IResponse failure)
        {
            switch (failure)
            {
                case ErrorResponse response:
                    _logger.Warning("Bad request error: {ErrorMessage}", response.Message);
                    return new BadRequestObjectResult(new ApiErrorResponse(Token, response));
                case ErrorResponses response:
                    _logger.Warning("Bad request errors: {ErrorMessages}",
                        string.Join(",", response.Errors.Select(errorResponse => errorResponse.Message)));
                    return new BadRequestObjectResult(new ApiErrorResponse(Token, response.Errors));
                case IFail response:
                    _logger.Warning("Fail errors: {ErrorMessages}", response.ErrorCode);
                    return new BadRequestObjectResult(
                        new ApiErrorResponse(Token, new ErrorResponse(response.ErrorCode)));
                default:
                    _logger.Error("Unknown response type: {ObjectName}", nameof(failure));
                    return new BadRequestObjectResult(
                        new ApiErrorResponse(Token, new InvalidResource(nameof(failure), "Unsupported object type")));
            }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public class ApiErrorResponse
        {
            public long Timestamp { get; }
            public string CorrelationId { get; }
            public IEnumerable<ErrorResponse> Errors { get; }

            public ApiErrorResponse(UserToken token, ErrorResponse error)
                : this(token, new[] { error })
            {
            }

            public ApiErrorResponse(UserToken token, IEnumerable<ErrorResponse> errors)
            {
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                CorrelationId = token.CorrelationId;
                Errors = errors;
            }
        }
        
        protected IActionResult RepositoryActionToResult<TModel>(RepositoryActionResultModel<TModel> theResult) 
            where TModel : class, IModel
        {
            return theResult.ResultType switch
            {
                ResultType.NotFound => NotFound(theResult),
                ResultType.Conflict => Conflict(theResult),
                ResultType.FailedValidation or ResultType.FailedVerification => BadRequest(theResult),
                _ => Ok(theResult)
            };
        }

        [Obsolete]
        protected IActionResult CommonResponseHandler<TExpectedType>(IResponse? response) 
            where TExpectedType : class, IResponse
        {
            return response switch
            {
                null => BadRequest(),
                _ => HandleResponse<TExpectedType>(response)
            };
        }
        
        protected IActionResult HandleResponse<TExpectedType>(IResponse response) 
            where TExpectedType : class, IResponse
        {
            return response switch
            {
                TExpectedType r => Ok(r),
                NotFoundResponse r => NotFound(r),
                ConflictResponse r => Conflict(r),
                IFail r => BadRequest(r),
                _ => throw new InvalidOperationException()
            };
        }
    }
}