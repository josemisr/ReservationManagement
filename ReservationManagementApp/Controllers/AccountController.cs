using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementApp.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
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
        private readonly ReservationManagementDbContext _context;

        public AccountController(ReservationManagementDbContext context)
        {
            _context = context;
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
            Users userDb = _context.Users.FirstOrDefault(elem => elem.Email == email && elem.Password == password);
            if (userDb != null)
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                {
                    HttpContext.Session.SetString("User", JsonSerializer.Serialize(userDb));
                    HttpContext.Session.SetString("UserName", userDb.Name);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Login incorrecto");
                return View();
            }
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
        public async Task<IActionResult> Register([Bind("Name,Surname,Surname2,IdCard,Email,Birthday,Password")] Users user)
        {
            if (ModelState.IsValid)
            {
                user.IdRole = 2;
                if (_context.Users.FirstOrDefault(elem => elem.Email == user.Email) != null)
                {
                    ModelState.AddModelError("ErrorEmail", "The Email already exists");
                    return View(user);
                }
                _context.Add(user);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                    {
                        HttpContext.Session.SetString("User", JsonSerializer.Serialize(user));
                        HttpContext.Session.SetString("UserName", user.Name);
                    }
                    return RedirectToAction("Index", "Home");
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
            }
            return RedirectToAction("Index", "Home");
        }
    }
}