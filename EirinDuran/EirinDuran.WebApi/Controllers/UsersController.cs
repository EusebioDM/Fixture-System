using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using EirinDuran.Domain.User;
using EirinDuran.WebApi.Models;
using EirinDuran.IServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EirinDuran.Services;
using System;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly IUserServices userServices;

        public UsersController(ILoginServices loginServices, IUserServices userServices)
        {
            this.userServices = userServices;
            this.loginServices = loginServices;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<User>> Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            List<Claim> claims = identity.Claims.ToList();

            string userName = claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
            string password = claims.Where(c => c.Type == "Password").Select(c => c.Value).SingleOrDefault();

            loginServices.CreateSession(userName, password);

            return userServices.GetAllUsers().ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<User> GetById(string id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            List<Claim> claims = identity.Claims.ToList();

            string userName = claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
            string password = claims.Where(c => c.Type == "Password").Select(c => c.Value).SingleOrDefault();

            loginServices.CreateSession(userName, password);

            User user = userServices.GetUser(new User(id));
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(UserModelIn userModel)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            List<Claim> claims = identity.Claims.ToList();

            string userName = claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
            string password = claims.Where(c => c.Type == "Password").Select(c => c.Value).SingleOrDefault();

            loginServices.CreateSession(userName, password);

            if (ModelState.IsValid)
            {
                return TryToAddUser(userModel);
                
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private IActionResult TryToAddUser(UserModelIn userModel)
        {
            try
            {
                //Poner una fábrica acá
                User user = new User(userModel.Role, userModel.UserName, userModel.Name, userModel.Surname, userModel.Password, userModel.Mail);
                userServices.AddUser(user);

                var addedUser = new UserModelOut() { UserName = user.UserName, Name = user.Name, Surname = user.Surname, Mail = user.Mail, Role = user.Role };
                return CreatedAtRoute("GetUser", new { id = addedUser.UserName }, addedUser);
            }
            catch(InsufficientPermissionToPerformThisActionException)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            List<Claim> claims = identity.Claims.ToList();

            string userName = claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
            string password = claims.Where(c => c.Type == "Password").Select(c => c.Value).SingleOrDefault();

            loginServices.CreateSession(userName, password);
            return TryToDelete(id);
        }

        private IActionResult TryToDelete(string id)
        {
            try
            {
                userServices.DeleteUser(id);
                return Ok();
            }
            catch(UserTryToDeleteDoesNotExistsException)
            {
                return BadRequest();
            }
        }
    }
}
