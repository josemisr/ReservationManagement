using AccountServiceApi.IServicesApi;
using AccountServiceApi.Models;
using AutoMapper;
using DataAccess.Models;
using DataAccess.Operations;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AccountServiceApi.ServicesApi
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration configuration;
        private UserOperations db = new UserOperations();
        private readonly IMapper _mapper;
        public AccountService(IConfiguration configuration, IMapper mapper)
        {
            this.configuration = configuration;
            this._mapper = mapper;
        }
        public string Login(UserLogin userLogin)
        {
            Users userDb = db.ValidateUserLogin(userLogin.Email, userLogin.Password);
            if (userDb != null)
            {
                UserJWT user = new UserJWT()
                {
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
        public List<UserSimpleDto> GetUsers()
        {
            var result = db.GetAllUsers();
            return _mapper.Map<List<Users>, List<UserSimpleDto>>(result);
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
