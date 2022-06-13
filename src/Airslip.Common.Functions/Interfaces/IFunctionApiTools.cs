using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Types.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;

namespace Airslip.Common.Functions.Interfaces
{
    public interface IFunctionApiTools
    {
        Task<bool> CanAuthenticate(HttpRequestData req,
            FunctionContext executionContext);

        Task<HttpResponseData> OkResponse<T>(HttpRequestData req, T response) 
            where T: class, IResponse;

        Task<HttpResponseData> NotFound<T>(HttpRequestData req, T response)
            where T: class, IResponse;

        Task<HttpResponseData> Unauthorised<T>(HttpRequestData req, T response)
            where T: class, IResponse;
            
        Task<HttpResponseData> Conflict<T>(HttpRequestData req, T response)
                    where T : class, IResponse;

        Task<HttpResponseData> BadRequest<T>(HttpRequestData req, T failure)
            where T: class, IResponse;

        Task<HttpResponseData> RepositoryActionToResult<TModel>(HttpRequestData req, RepositoryActionResultModel<TModel> theResult) 
            where TModel : class, IModel;

        Task<HttpResponseData> CommonResponseHandler<TExpectedType>(HttpRequestData req, IResponse response) 
            where TExpectedType : class, IResponse;
    }
}