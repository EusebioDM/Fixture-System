using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using EirinDuran.WebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using EirinDuran.IServices.Interfaces;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private ILoginServices loginServices;
        private IConfiguration configuration;

        public AuthController(IConfiguration configuration, ILoginServices loginServices)
        {
            this.loginServices = loginServices;
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
                return BadRequest(e);
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
            return Ok(new { Token = tokenString });
        }
    }
}
