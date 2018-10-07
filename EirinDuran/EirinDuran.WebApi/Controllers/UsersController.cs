using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.WebApi.Mappers;
using EirinDuran.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly IUserServices userServices;
        private readonly IEncounterServices encounterServices;

        public UsersController(ILoginServices loginServices, IUserServices userServices, IEncounterServices encounterServices)
        {
            this.userServices = userServices;
            this.loginServices = loginServices;
            this.encounterServices = encounterServices;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Follower")]
        public ActionResult<List<UserModelOut>> GetAll()
        {
            CreateSession();
            try
            {
                return TryToGetAllUsers();
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
            catch (ServicesException e)
            {
                return BadRequest(e);
            }
        }


        private ActionResult<List<UserModelOut>> TryToGetAllUsers()
        {
            return userServices.GetAllUsers().Select(u => new UserModelOut(u)).ToList();
        }


        [HttpGet("{userId}", Name = "GetUser")]
        [Authorize(Roles = "Administrator, Follower")]
        public ActionResult<UserModelOut> GetById(string userId)
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
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }


        private ActionResult<UserModelOut> TryToGetById(string userId)
        {
            UserDTO user = userServices.GetUser(userId);
            return new UserModelOut(user);
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
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        private IActionResult TryToAddUser(UserModelIn userModel)
        {
            UserDTO user = UserMapper.Map(userModel);
            userServices.CreateUser(user);

            var addedUser = new UserModelOut(user);
            return CreatedAtRoute("GetUser", new { id = addedUser.UserName }, addedUser);
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
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        private IActionResult TryToModify(string userId, UserUpdateModelIn userModel)
        {
            UserDTO user = UserMapper.Map(userModel);
            user.UserName = userId;

            userServices.ModifyUser(user);
            return Ok();

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
            catch (ServicesException)
            {
                return BadRequest();
            }
        }

        private IActionResult TryToDelete(string userId)
        {
            userServices.DeleteUser(userId);
            return Ok();
        }

        [HttpGet]
        [Route("followers")]
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

        [HttpGet]
        [Route("commentaries")]
        [Authorize]
        public ActionResult<List<CommentDTO>> GetFollowedTeamCommentaries()
        {
            CreateSession();
            try
            {
                UserDTO user = userServices.GetUser(loginServices.LoggedUser.UserName);
                IEnumerable<EncounterDTO> encounters = encounterServices.GetAllEncountersWithFollowedTeams();
                List<CommentDTO> comments = new List<CommentDTO>();
                foreach (var encounter in encounters)
                {
                    comments.AddRange(encounterServices.GetAllCommentsToOneEncounter(encounter.Id.ToString()));
                }
                return comments;
            }
            catch (ServicesException ex)
            {
                return BadRequest(ex.Message);
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
