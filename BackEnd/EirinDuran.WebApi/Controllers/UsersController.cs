using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using EirinDuran.IServices.Services_Interfaces;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly IUserServices userServices;
        private readonly IEncounterSimpleServices _encounterSimpleServices;
        private readonly IEncounterQueryServices encounterQueryServices;

        public UsersController(ILoginServices loginServices, IUserServices userServices, IEncounterSimpleServices encounterSimpleServices, IEncounterQueryServices encounterQueryServices)
        {
            this.userServices = userServices;
            this.loginServices = loginServices;
            this._encounterSimpleServices = encounterSimpleServices;
            this.encounterQueryServices = encounterQueryServices;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Follower")]
        public ActionResult<List<UserModelOut>> GetAll()
        {
            try
            {
                CreateSession();
                return TryToGetAllUsers();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
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
            try
            {
                CreateSession();
                return TryToGetById(userId);
            }
            catch (ServicesException e)
            {
                return NotFound(e.Message);
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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                CreateSession();
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
            UserDTO user = userModel.ToServicesDTO();
            userServices.CreateUser(user);

            var addedUser = new UserModelOut(user);
            return CreatedAtRoute("GetUser", new { userId = addedUser.UserName }, addedUser);
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Modify(string userId, [FromBody] UserUpdateModelIn userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                CreateSession();
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
            UserDTO user = userModel.ToServicesDTO();
            user.UserName = userId;

            userServices.ModifyUser(user);
            return Ok();
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string userId)
        {
            try
            {
                CreateSession();
                return TryToDelete(userId);
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
            try
            {
                CreateSession();
                return loginServices.LoggedUser.FollowedTeamsNames;
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("commentaries")]
        [Authorize(Roles = "Administrator, Follower")]
        public ActionResult<List<CommentDTO>> GetFollowedTeamCommentaries()
        {
            try
            {
                CreateSession();
                UserDTO user = userServices.GetUser(loginServices.LoggedUser.UserName);
                IEnumerable<EncounterDTO> encounters = encounterQueryServices.GetAllEncountersWithFollowedTeams();
                List<CommentDTO> comments = new List<CommentDTO>();
                foreach (var encounter in encounters)
                {
                    comments.AddRange(encounterQueryServices.GetAllCommentsToOneEncounter(encounter.Id.ToString()));
                }
                return comments;
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
