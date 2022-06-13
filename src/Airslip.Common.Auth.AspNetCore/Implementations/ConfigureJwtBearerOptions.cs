using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.AspNetCore.Implementations
{
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtSettings _settings;
        private readonly SigningCredentials _signingCredentials;
        private readonly ILogger _logger;

        public ConfigureJwtBearerOptions(IOptions<JwtSettings> settings, ILogger logger)
        {
            _settings = settings.Value;
            _logger = logger;
            _signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
                SecurityAlgorithms.HmacSha256);
        }

        public void Configure(JwtBearerOptions options)
        {
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                    else
                    {
                        _logger.Error(
                            "Unhandled JwtBearerToken error with exception type {ExceptionType}", 
                            context.Exception.GetType().Name);
                    }

                    return Task.CompletedTask;
                }
            };
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,
                IssuerSigningKey = _signingCredentials.Key,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
                ValidateLifetime = _settings.ValidateLifetime
            };
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            Configure(options);
        }
    }
}