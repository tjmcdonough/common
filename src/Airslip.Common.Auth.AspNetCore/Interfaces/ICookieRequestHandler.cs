using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.AspNetCore.Interfaces
{
    public interface ICookieRequestHandler
    {
        Task<KeyAuthenticationResult> Handle(HttpRequest requestData);
    }
}