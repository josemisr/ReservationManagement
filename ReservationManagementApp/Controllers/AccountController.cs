using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReservationManagementApp.Models.Dto;
using ReservationManagementApp.Models.JWT;
using ReservationManagementApp.ServicesApp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationManagementApp.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public HttpServicesReponse _clientService = new HttpServicesReponse();
        public AccountController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        ////
        //// POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(String email, string password, string returnUrl)
        {
            string content = "{\"Email\":\"" + email + "\",\"Password\":\"" + password + "\"}";
            string responseBody = this._clientService.PostResponse(this._configuration["AppSettings:ApiRest"] + "api/AccountApi", content).GetAwaiter().GetResult();
            JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(responseBody))
            {
                var tokenS = hand.ReadJwtToken(responseBody);
                if (responseBody != null)
                {
                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                    {
                        UserJWT userJWT = JsonConvert.DeserializeObject<UserJWT>(tokenS.Payload["UserJwt"].ToString());
                        UserDto userDto = new UserDto();
                        userDto.Email = userJWT.Email;
                        userDto.IdRole = userJWT.IdRole;
                        userDto.Name = userJWT.Name;
                        userDto.Surname = userJWT.Surname;
                        userDto.Surname2 = userJWT.Surname2;
                        userDto.Id = userJWT.Id;
                        HttpContext.Session.SetString("User", JsonConvert.SerializeObject(userDto));
                        HttpContext.Session.SetString("Jwt", JsonConvert.SerializeObject(responseBody));
                        HttpContext.Session.SetString("UserName", userDto.Name);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Login incorrecto");
            return View();
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name,Surname,Surname2,IdCard,Email,Birthday,Password")] UserDto user)
        {
            if (ModelState.IsValid)
            {
                user.IdRole = 2;
                string responseBody = await this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/AccountApi");
                List<UserDto> list = JsonConvert.DeserializeObject<List<UserDto>>(responseBody);
                if (list.FirstOrDefault(elem => elem.Email == user.Email) != null)
                {
                    ModelState.AddModelError("ErrorEmail", "The Email already exists");
                    return View(user);
                }
                string responseBody2 = await this._clientService.PostResponse(this._configuration["AppSettings:ApiRest"] + "api/AccountApi/Register", JsonConvert.SerializeObject(user));
                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                if (!string.IsNullOrEmpty(responseBody2))
                {
                    var tokenS = hand.ReadJwtToken(responseBody2);
                    if (responseBody2 != null)
                    {
                        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                        {
                            UserJWT userJWT = JsonConvert.DeserializeObject<UserJWT>(tokenS.Payload["UserJwt"].ToString());
                            UserDto userDto = new UserDto();
                            userDto.Email = userJWT.Email;
                            userDto.IdRole = userJWT.IdRole;
                            userDto.Name = userJWT.Name;
                            userDto.Surname = userJWT.Surname;
                            userDto.Surname2 = userJWT.Surname2;
                            userDto.Id = userJWT.Id;
                            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(userDto));
                            HttpContext.Session.SetString("Jwt", JsonConvert.SerializeObject(responseBody2));
                            HttpContext.Session.SetString("UserName", user.Name);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(user);
        }

        ////
        //// POST: /Account/LogOff
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                HttpContext.Session.SetString("User", "");
                HttpContext.Session.SetString("UserName", "");
                HttpContext.Session.SetString("Jwt", "");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}