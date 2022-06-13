using Microsoft.Azure.Functions.Worker.Http;
using System.Collections.Specialized;
using System.Security.Claims;

namespace Airslip.Common.Auth.Functions.Interfaces
{
    public interface IFunctionContextAccessor
    {
        NameValueCollection? QueryString { get; set; }
        HttpHeadersCollection? Headers { get; set; }
        ClaimsPrincipal? User { get; set; }
    }
}