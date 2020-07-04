using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace FunctionsReservation
{
    public static class SecurityJwt
    {
        public static ClaimsPrincipal ValidateToken(AuthenticationHeaderValue value, string context)
        {
            if (value?.Scheme != "Bearer")
                return null;

            var configFile = new ConfigurationBuilder()
            .SetBasePath(context)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            var validationParameter = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidAudience = configFile["JWT:Audience"],
                ValidateAudience = true,
                ValidIssuer = configFile["JWT:Issuer"],
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configFile["JWT:SecretKey"]))
            };

            ClaimsPrincipal result = null;
            
            try
            {
                var handler = new JwtSecurityTokenHandler();
                result = handler.ValidateToken(value.Parameter, validationParameter, out var token);
            }

            catch (SecurityTokenException ex)
            {
                return null;
            }
            return result;
        }
    }
}