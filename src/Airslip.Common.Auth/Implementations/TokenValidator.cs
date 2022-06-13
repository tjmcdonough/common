using Airslip.Common.Auth.Exceptions;
using Airslip.Common.Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Implementations
{
    public class TokenValidator<TExisting> : ITokenValidator<TExisting> 
        where TExisting : IDecodeToken, new()
    {
        private readonly ITokenDecodeService<TExisting> _tokenService;

        public TokenValidator(ITokenDecodeService<TExisting> tokenService)
        {
            _tokenService = tokenService;
        }
        
        public async Task<ClaimsPrincipal?> GetClaimsPrincipalFromToken(string value, string forScheme, 
            string forEnvironment)
        {
            Tuple<TExisting, ICollection<Claim>> tokenDetails = _tokenService.DecodeToken(value);

            if (tokenDetails.Item1.Environment != forEnvironment)
            {
                throw new EnvironmentUnsupportedException(tokenDetails.Item1.Environment, 
                    forEnvironment);
            }

            List<ClaimsIdentity> claimsIdentities = new()
            {
                new ClaimsIdentity(tokenDetails.Item2, forScheme)
            };
            
            ClaimsPrincipal principal = new(claimsIdentities);
            
            return await Task.FromResult(principal);
        }
    }
}