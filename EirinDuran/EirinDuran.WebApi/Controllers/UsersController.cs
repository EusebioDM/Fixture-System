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
using System;

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
                return BadRequest(e);
            }
        }

        [HttpGet("{userId}", Name = "GetUser")]
        [Authorize(Roles = "Administrator, Follower")]
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

                var addedUser = new UserModelOut(user);
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
        public ActionResult<List<CommentDTO>> GetFollowedTeamCommentaries()
        {
            CreateSession();
            try
            {
                UserDTO user = userServices.GetUser(loginServices.LoggedUser.UserName);
                IEnumerable<EncounterDTO> encounters = encounterServices.GetAllEncountersWithFollowedTeams();
                List<CommentDTO> comments = new List<CommentDTO>();
                foreach(var encounter in encounters)
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
