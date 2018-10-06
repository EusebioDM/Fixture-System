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
using EirinDuran.IServices;

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
            catch (InsufficientPermissionException)
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
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{userId}", Name = "GetUser")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<UserDTO> GetById(string userId)
        {
            CreateSession();
            try
            {
                return TryToGetById(userId);
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private ActionResult<UserDTO> TryToGetById(string userId)
        {
            try
            {
                return userServices.GetUser(userId);
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
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
            catch (InsufficientPermissionException)
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
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Modify(string userId, [FromBody] UserUpdateModelIn userModel)
        {
            CreateSession();
            try
            {
                return TryToModify(userId, userModel);
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToModify(string userId, UserUpdateModelIn userModel)
        {
            try
            {
                UserDTO user = UserMapper.Map(userModel);
                user.UserName = userId;

                userServices.ModifyUser(user);
                return Ok();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string userId)
        {
            CreateSession();
            try
            {
                return TryToDelete(userId);
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToDelete(string userId)
        {
            try
            {
                userServices.DeleteUser(userId);
                return Ok();
            }
            catch (ServicesException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/followers")]
        [Authorize(Roles = "Administrator, Follower")]
        public ActionResult<List<string>> GetFollowedTeams()
        {
            CreateSession();
            try
            {
                return loginServices.LoggedUser.FollowedTeamsNames;
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("followers/{id}")]
        [Authorize(Roles = "Administrator, Follower")]
        public IActionResult AddFollowedTeamToLogedUser(string id)
        {
            CreateSession();
            try
            {
                userServices.AddFollowedTeam(id);
                return Ok();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
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
