using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace Airslip.Common.Auth.Implementations
{
    public class RemoteIpAddressService : IRemoteIpAddressService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoteIpAddressService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetRequestIP()
        {
            string? ip = GetHeaderValueAsString("X-Forwarded-For").SplitCsv().FirstOrDefault();

            if (ip.IsNullOrWhitespace() && _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null)
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAsString("REMOTE_ADDR");

            if (ip.IsNullOrWhitespace())
                throw new ArgumentException("Unable to determine caller's IP.");

            return ip;
        }

        private string GetHeaderValueAsString(string headerName)
        {
            StringValues values = default;

            if (_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString(); // writes out as Csv when there are multiple.

                if (!rawValues.IsNullOrWhitespace())
                    return rawValues;
            }

            return "";
        }
        
    }
}