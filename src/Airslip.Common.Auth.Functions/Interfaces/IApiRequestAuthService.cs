using Airslip.Common.Auth.Models;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Functions.Interfaces
{
    public interface IApiRequestAuthService
    {
        Task<KeyAuthenticationResult> Handle(HttpRequestData requestData);
        Task<KeyAuthenticationResult> Handle(string functionNamed, HttpRequestData requestData);
    }
}