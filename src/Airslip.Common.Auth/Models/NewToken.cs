using System;

namespace Airslip.Common.Auth.Models
{
    public record NewToken(string TokenValue, DateTime? TokenExpiry);
}