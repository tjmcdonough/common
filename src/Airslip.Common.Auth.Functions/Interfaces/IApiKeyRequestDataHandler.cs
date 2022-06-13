using Airslip.Common.Auth.Models;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;

namespace Airslip.Common.Auth.Functions.Interfaces
{
    public interface IApiKeyRequestDataHandler
    {
        Task<KeyAuthenticationResult> Handle(HttpRequestData request);
    }
}