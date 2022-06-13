using Airslip.Common.Auth.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Interfaces
{
    public interface ITokenGenerationService<TGenerateTokenType> 
        where TGenerateTokenType : IGenerateToken
    {
        NewToken GenerateNewToken(ICollection<Claim> claims);
        
        NewToken GenerateNewToken(TGenerateTokenType token);
    }
}