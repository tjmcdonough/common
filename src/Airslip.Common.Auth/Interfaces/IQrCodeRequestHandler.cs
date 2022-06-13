using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Interfaces
{
    public interface IQrCodeRequestHandler
    {
        Task<KeyAuthenticationResult> Handle(HttpRequest request);
    }
}