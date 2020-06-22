using System;
using System.Collections.Generic;
using AccountServiceApi.IServicesApi;
using AccountServiceApi.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountServiceApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountServiceController : ControllerBase
    {
        IAccountService _accountServices;

        public AccountServiceController(IAccountService accountService)
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
        public IActionResult GetUsers()
        {
            List<UserSimpleDto> usersDb = _accountServices.GetUsers();
            return Ok(usersDb);
        }
    }
}