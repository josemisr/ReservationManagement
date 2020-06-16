using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReservationManagementApp.Models;
using ReservationManagementApp.Models.Dto;
using ReservationManagementApp.Models.JWT;
using ReservationManagementApp.ServicesApp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security;


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
            string content = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
            string responseBody = _clientService.PostResponse(this._configuration["AppSettings:ApiRest"] + "api/AccountApi", content).GetAwaiter().GetResult();
            JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(responseBody))
            {
                var tokenS = hand.ReadJwtToken(responseBody);
                if (responseBody != null)
                {
                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                    {
                        UserJWT userJWT = JsonSerializer.Deserialize<UserJWT>(tokenS.Payload["UserJwt"].ToString());
                        UserDto userDb = new UserDto();
                        userDb.Email = userJWT.Email;
                        userDb.IdRole = userJWT.IdRole;
                        userDb.Name = userJWT.Name;
                        userDb.Surname = userJWT.Surname;
                        userDb.Surname2 = userJWT.Surname2;
                        userDb.Id = userJWT.Id;
                        HttpContext.Session.SetString("User", JsonSerializer.Serialize(userDb));
                        HttpContext.Session.SetString("Jwt", JsonSerializer.Serialize(responseBody));
                        HttpContext.Session.SetString("UserName", userDb.Name);
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
                string responseBody = await _clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/AccountApi");
                List<UserDto> list = JsonSerializer.Deserialize<List<UserDto>>(responseBody);
                if (list.FirstOrDefault(elem => elem.Email == user.Email) != null)
                {
                    ModelState.AddModelError("ErrorEmail", "The Email already exists");
                    return View(user);
                }
                string responseBody2 = await _clientService.PostResponse(this._configuration["AppSettings:ApiRest"] + "api/AccountApi/Register", JsonSerializer.Serialize(user));
                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                if (!string.IsNullOrEmpty(responseBody2))
                {
                    var tokenS = hand.ReadJwtToken(responseBody2);
                    if (responseBody2 != null)
                    {
                        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                        {
                            HttpContext.Session.SetString("User", JsonSerializer.Serialize(user));
                            HttpContext.Session.SetString("Jwt", JsonSerializer.Serialize(responseBody2));
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