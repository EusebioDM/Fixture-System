using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SilverFixture.IServices.Infrastructure_Interfaces;
using SilverFixture.IServices.Services_Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using SilverFixture.WebApi.Models;

namespace SilverFixture.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ILoginServices loginServices;
        private readonly ILogger logger;
        private readonly IConfiguration configuration;

        public AuthController(IConfiguration configuration, ILoginServices loginServices, ILogger logger)
        {
            this.loginServices = loginServices;
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody]LoginModelIn loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return TryToLogin(loginModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private IActionResult TryToLogin(LoginModelIn loginModel)
        {
            loginServices.CreateSession(loginModel.UserName, loginModel.Password);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Secret"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: new List<Claim>{
                    new Claim(ClaimTypes.Role, loginServices.LoggedUser.IsAdmin ? "Administrator" : "Follower"),
                    new Claim("UserName", loginModel.UserName),
                    new Claim("Password", loginModel.Password),
                },
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            logger.Log(loginModel.UserName, "Logged into the system");
            return Ok(new { Token = tokenString });
        }
    }
}
