using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Interfaces
{
    public interface ITokenDecodeService<TTokenType> 
        where TTokenType : IDecodeToken, new()
    {
        TTokenType GetCurrentToken();
        Tuple<TTokenType, ICollection<Claim>> DecodeToken(string tokenValue);
        Tuple<TTokenType, ICollection<Claim>> DecodeTokenFromHeader(string headerValue);
        Tuple<TTokenType, ICollection<Claim>> DecodeTokenFromHeader(string headerValue, string withScheme);
    }
}