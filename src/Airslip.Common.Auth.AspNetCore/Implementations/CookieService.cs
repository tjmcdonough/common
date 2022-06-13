using Airslip.Common.Auth.AspNetCore.Configuration;
using Airslip.Common.Auth.AspNetCore.Interfaces;
using Airslip.Common.Auth.AspNetCore.Schemes;
using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Security.Implementations;
using Airslip.Common.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.AspNetCore.Implementations
{
    public class CookieService : ICookieService
    {
        private readonly ITokenGenerationService<GenerateUserToken> _tokenGenerationService;
        private readonly ITokenValidator<UserToken> _tokenValidator;
        private readonly ILogger _logger;
        private readonly CookieSettings _cookieSettings;
        private readonly HttpContext _context;

        public CookieService(ITokenGenerationService<GenerateUserToken> tokenGenerationService,
            ITokenValidator<UserToken> tokenValidator,
            IHttpContextAccessor httpContextAccessor,
            ILogger logger,
            IOptions<CookieSettings> cookieSettings)
        {
            _tokenGenerationService = tokenGenerationService;
            _tokenValidator = tokenValidator;
            _logger = logger;
            _cookieSettings = cookieSettings.Value;
            _context = httpContextAccessor.HttpContext ?? 
                       throw new ArgumentException("HttpContext cannot be null");
        }
        
        public async Task UpdateCookie(GenerateUserToken generateUserToken)
        {
            NewToken newToken = _tokenGenerationService.GenerateNewToken(generateUserToken);
            
            // Add to the response as a new cookie
            string random = CommonFunctions.GetId();
            string passphrase = $"{_cookieSettings.Passphrase}{random}";
            string base64encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(random));
            CookieOptions options = new()
            {
                Secure = true,
                SameSite = SameSiteMode.None
            };
            
            _context.Response.Cookies.Append(CookieSchemeOptions.CookieEncryptField, base64encoded, options);
            _context.Response.Cookies.Append(CookieSchemeOptions.CookieTokenField, 
                StringCipher.Encrypt(newToken.TokenValue, passphrase), options);
            
            // Update the claims principal with this cookie
            ClaimsPrincipal? claimsPrincipal = await _tokenValidator.GetClaimsPrincipalFromToken(newToken.TokenValue, 
                CookieAuthenticationSchemeOptions.CookieAuthScheme, 
                AirslipSchemeOptions.ThisEnvironment);
        
            // Use the generated claims principal in this request
            _context.User = claimsPrincipal ?? new ClaimsPrincipal();
        }

        public string GetCookieValue(HttpRequest request)
        {
            string cookieValue = request.Cookies[CookieSchemeOptions.CookieTokenField] ?? string.Empty;
            string base64encoded = request.Cookies[CookieSchemeOptions.CookieEncryptField] ?? string.Empty;
            string passphrase = Encoding.UTF8.GetString(Convert.FromBase64String(base64encoded));
            
            // Otherwise, decrypt!
            string wholePassphrase = $"{_cookieSettings.Passphrase}{passphrase}";
            
            // Decrypt token
            string decryptedToken;
            try
            {
                decryptedToken = StringCipher.Decrypt(cookieValue, wholePassphrase);
            }
            catch (Exception ee)
            {
                _logger.Error(ee, "Error decoding cookie");
                throw new ArgumentException("Cookie invalid");
            }

            return decryptedToken;
        }
    }
}