using Api.IServicesApi;
using Api.Models;
using DataAccess.Models;
using DataAccess.Operations;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.ServicesApi
{
    public class AccountService:IAccountService
    {
        private readonly IConfiguration configuration;
        private UserOperations db = new UserOperations();

        public AccountService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Login(UserLogin userLogin)
        {
            Users userDb = db.ValidateUserLogin(userLogin.Email, userLogin.Password);
            if (userDb != null)
            {
                UserJWT user = new UserJWT()
                {
                    // Id del Usuario en el Sistema de Información (BD)
                    Id = userDb.Id,
                    Name = userDb.Name,
                    Surname = userDb.Surname,
                    Surname2 = userDb.Surname2,
                    Email = userDb.Email,
                    IdRole = userDb.IdRole,
                    Rol = userDb.IdRoleNavigation.Name,
                };
                return GenerarTokenJWT(user);
            }
            else
            {
                return string.Empty;
            }
        }

        public string Register(Users user)
        {
            user.IdRole = 2;
            Users userDb = db.CreateUser(user);
            if (userDb != null)
            {
                UserJWT userJwt = new UserJWT()
                {
                    // Id del Usuario en el Sistema de Información (BD)
                    Id = userDb.Id,
                    Name = userDb.Name,
                    Surname = userDb.Surname,
                    Surname2 = userDb.Surname2,
                    Email = userDb.Email,
                    IdRole = userDb.IdRole,
                    Rol = "Client",
                };
                return GenerarTokenJWT(userJwt);
            }
            else
            {
                return String.Empty;
            }
        }

        public List<Users> GetUsers()
        {
            return db.GetAllUsers();
        }
        private string GenerarTokenJWT(UserJWT userJwt)
        {
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var header = new JwtHeader(_signingCredentials);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, userJwt.Id.ToString()),
                new Claim("Name", userJwt.Name),
                new Claim("Surname", userJwt.Surname),
                new Claim("UserJwt",  JsonSerializer.Serialize(userJwt)),
                new Claim(ClaimTypes.Role, userJwt.Rol)
            };

            var payload = new JwtPayload(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddHours(5)
                );

            var token = new JwtSecurityToken(
                    header,
                    payload
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
