using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.AspNetCore.Interfaces
{
    public interface ICookieService
    {
        Task UpdateCookie(GenerateUserToken userToken);
        string GetCookieValue(HttpRequest request);
    }
}