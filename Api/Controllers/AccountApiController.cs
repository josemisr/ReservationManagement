using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Api.IServicesApi;
using Api.Models;
using DataAccess.Models;
using DataAccess.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        IAccountService _accountServices;

        public AccountApiController(IAccountService accountService)
        {
            this._accountServices = accountService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var result = this._accountServices.Login(userLogin);

            if (!String.IsNullOrEmpty(result))
            {  
                return Ok(result);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] Users user)
        {
            var result = this._accountServices.Register(user);

            if (!String.IsNullOrEmpty(result))
            {
                return Ok(result);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        public List<Users> Get()
        {
            List<Users> usersDb = _accountServices.GetUsers();
            return usersDb;
        }

       
    }
}