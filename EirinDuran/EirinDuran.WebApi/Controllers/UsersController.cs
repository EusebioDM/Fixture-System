using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using EirinDuran.WebApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EirinDuran.Services;
using EirinDuran.IServices.Interfaces;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.DTOs;
using EirinDuran.WebApi.Mappers;

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
        public ActionResult<List<UserDTO>> Get()
        {
            CreateSession();
            return userServices.GetAllUsers().ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<UserDTO> GetById(string id)
        {
            CreateSession();
            try
            {
                return userServices.GetUser(id);
            }
            catch(UserTryToRecoverDoesNotExistsException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(UserModelIn userModel)
        {
            CreateSession();
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
                UserDTO user = UserMapper.Map(userModel);
                userServices.CreateUser(user);

                var addedUser = new UserModelOut() { UserName = user.UserName, Name = user.Name, Surname = user.Surname, Mail = user.Mail, IsAdmin = user.IsAdmin };
                return CreatedAtRoute("GetUser", new { id = addedUser.UserName }, addedUser);
            }
            catch(InsufficientPermissionException)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Put(string id, [FromBody] UserModelIn userModel)
        {
            CreateSession();
            UserDTO user = UserMapper.Map(userModel);
            userServices.Modify(user);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string id)
        {
            CreateSession();
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

        private void CreateSession()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            List<Claim> claims = identity.Claims.ToList();

            string userName = claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
            string password = claims.Where(c => c.Type == "Password").Select(c => c.Value).SingleOrDefault();

            loginServices.CreateSession(userName, password);
        }
    }
}
