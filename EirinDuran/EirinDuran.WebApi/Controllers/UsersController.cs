using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using EirinDuran.WebApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
            try
            {
                return TryToGet();
            }
            catch(InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private ActionResult<List<UserDTO>> TryToGet()
        {
            try
            {
                return userServices.GetAllUsers().ToList();
            }
            catch (UserServicesException)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}", Name = "GetUser")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<UserDTO> GetById(string id)
        {
            CreateSession();
            try
            {
                return TryToGetById(id);
            }
            catch(InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private ActionResult<UserDTO> TryToGetById(string id)
        {
            try
            {
                return userServices.GetUser(id);
            }
            catch (UserServicesException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(UserModelIn userModel)
        {
            CreateSession();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                return TryToAddUser(userModel);
            }
            catch(InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToAddUser(UserModelIn userModel)
        {
            try
            {
                UserDTO user = UserMapper.Map(userModel);
                userServices.CreateUser(user);

                var addedUser = new UserModelOut() { UserName = user.UserName, Name = user.Name, Surname = user.Surname, Mail = user.Mail, IsAdmin = user.IsAdmin };
                return CreatedAtRoute("GetUser", new { id = addedUser.UserName }, addedUser);
            }
            catch(UserServicesException)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Modify(string id, [FromBody] UserUpdateModelIn userModel)
        {
            CreateSession();
            try
            {
                return TryToModify(userModel);
            }
            catch(InsufficientPermissionException)
            {
                return Unauthorized();
            }
            
        }

        private IActionResult TryToModify(UserUpdateModelIn userModel)
        {
            try
            {
                UserDTO user = UserMapper.Map(userModel);
                userServices.ModifyUser(user);
                return Ok();
            }
            catch (UserServicesException)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string id)
        {
            CreateSession();
            try
            {
                return TryToDelete(id);
            }
            catch(InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToDelete(string id)
        {
            try
            {
                userServices.DeleteUser(id);
                return Ok();
            }
            catch(UserServicesException)
            {
                return BadRequest();
            }
        }

        [HttpPut("follow/{id}")]
        [Authorize(Roles = "Administrator, Follower")]
        public IActionResult AddFollowedTeamToLogedUser(string id)
        {
            CreateSession();
            try
            {
                userServices.AddFollowedTeam(id);
                return Ok();
            }
            catch (UserServicesException)
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
