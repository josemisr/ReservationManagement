using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;
using DataAccess.Operations;
using System.Text;
using System.Security.Claims;
using System.Text.Json;
using FunctionsUser.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using AutoMapper;

namespace FunctionsUser
{
    public class FunctionsUserApi
    {
        private UserOperations db = new UserOperations();
        IMapper _mapper;

        public FunctionsUserApi(IMapper mapper)
        {
            _mapper = mapper;
        }

        [FunctionName("Login")]
        public async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "AccountFunctionApi")] HttpRequest req,
            ExecutionContext context)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            UserLogin userLogin = JsonSerializer.Deserialize<UserLogin>(content);
            string responseMessage = string.Empty;
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
                responseMessage = GenerarTokenJWT(user, context);
            }
            else
            {
                responseMessage = string.Empty;
            }
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("Register")]
        public async Task<IActionResult> Register(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = "AccountFunctionApi/Register")] HttpRequest req,
           ExecutionContext context)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            Users user = JsonSerializer.Deserialize<Users>(content);
            user.IdRole = 2;
            string responseMessage = String.Empty;
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
                responseMessage = GenerarTokenJWT(userJwt, context);
            }
            else
            {
                responseMessage = String.Empty;
            }
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetUsers")]
        public IActionResult GetUsers(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "AccountFunctionApi")] HttpRequest req)
        {
            var result = db.GetAllUsers();
            List<UserSimpleDto> responseMessage = _mapper.Map<List<Users>, List<UserSimpleDto>>(result);
            return new OkObjectResult(responseMessage);
        }

        private string GenerarTokenJWT(UserJWT userJwt, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
             .SetBasePath(context.FunctionAppDirectory)
             .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
             .AddEnvironmentVariables()
             .Build();
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["JWT:SecretKey"])
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var header = new JwtHeader(_signingCredentials);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, userJwt.Id.ToString()),
                new Claim("Name", userJwt.Name),
                new Claim("Surname", userJwt.Surname!=null?userJwt.Surname:""),
                new Claim("UserJwt",  JsonSerializer.Serialize(userJwt)),
                new Claim(ClaimTypes.Role, userJwt.Rol)
            };

            var payload = new JwtPayload(
                    issuer: config["JWT:Issuer"],
                    audience: config["JWT:Audience"],
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
