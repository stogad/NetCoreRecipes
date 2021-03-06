﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CookiesDemo.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CookiesDemo.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        public AccountController(ILogger logger)
        {
            _logger = logger;
        }
        // GET: api/<controller>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserInputModel userInput)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Incorrect credentials !");
            }
            _logger.Information("User {Name} logged out at {Time}.", User.Identity.Name, DateTime.UtcNow);

            //For test purpose Fake users are used. User credentials are not stored and retrived from Db and not validated.
            var user = new ApplicationUser
            {
                DateOfBirth = userInput.DateOfBirth.Value,
                Email = userInput.Email,
                FullName = userInput.FullName
            };

            var role = user.Email == "admin@abc.com" ? "Admin" : "User";

            var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("FullName", user.FullName),
                new Claim(ClaimTypes.Role, role),
                new Claim("LastChanged", DateTime.Now.ToString()),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(15)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok();
        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
