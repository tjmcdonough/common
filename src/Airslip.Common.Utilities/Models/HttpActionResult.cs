using Airslip.Common.Types.Interfaces;
using System.Net;

namespace Airslip.Common.Utilities.Models
{
    public record HttpActionResult(bool IsSuccessStatusCode, HttpStatusCode StatusCode, 
        string Content, IResponse? Response = null);

}