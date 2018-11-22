using SilverFixture.IServices.DTOs;
using SilverFixture.IServices.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using SilverFixture.IServices.Services_Interfaces;
using Microsoft.AspNetCore.Cors;
using SilverFixture.WebApi.Models;

namespace SilverFixture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly IUserServices userServices;
        private readonly IEncounterSimpleServices encounterSimpleServices;
        private readonly IEncounterQueryServices encounterQueryServices;

        public UsersController(ILoginServices loginServices, IUserServices userServices, IEncounterSimpleServices encounterSimpleServices, IEncounterQueryServices encounterQueryServices)
        {
            this.userServices = userServices;
            this.loginServices = loginServices;
            this.encounterSimpleServices = encounterSimpleServices;
            this.encounterQueryServices = encounterQueryServices;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Follower")]
        public ActionResult<List<UserModelOut>> GetAllUsers()
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
        public ActionResult<UserModelOut> GetUserById(string userId)
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
        public IActionResult CreateUser(UserModelIn userModel)
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
        [EnableCors("AllowAllOrigins")]
        [Authorize(Roles = "Administrator")]
        public IActionResult ModifyUser(string userId, [FromBody] UserUpdateModelIn userModel)
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
        public IActionResult DeleteUser(string userId)
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
        [Authorize]
        public ActionResult<List<TeamModelOut>> GetFollowedTeams()
        {
            try
            {
                CreateSession();
                return userServices
                    .GetFollowedTeams()
                    .Select(dto => new TeamModelOut(dto))
                    .ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet]
        [Route("encounters")]
        [Authorize]
        public ActionResult<List<EncounterModelOut>> GetFollowedTeamEncountersOfLoggedUser()
        {
            try
            {
                CreateSession();
                List<EncounterModelOut> encounters = new List<EncounterModelOut>();
                IEnumerable<TeamDTO> followedTeams = userServices.GetFollowedTeams();
                foreach (TeamDTO team in followedTeams)
                {
                    IEnumerable<EncounterModelOut> teamEncounters = encounterQueryServices
                        .GetEncountersByTeam(team.Name + "_" + team.SportName)
                        .Select(dto => new EncounterModelOut(dto));
                    encounters.AddRange(teamEncounters);
                }

                return encounters.ToHashSet().ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("commentaries")]
        [Authorize(Roles = "Administrator, Follower")]
        public ActionResult<List<CommentDTO>> GetFollowedTeamCommentariesOfLoggedUser()
        {
            try
            {
                CreateSession();
                UserDTO user = userServices.GetUser(loginServices.LoggedUser.UserName);
                IEnumerable<EncounterDTO> encounters = encounterQueryServices.GetAllEncountersWithFollowedTeams();
                List<CommentDTO> comments = new List<CommentDTO>();
                foreach (EncounterDTO encounter in encounters)
                {
                    comments.AddRange(encounterQueryServices.GetAllCommentsToOneEncounter(encounter.Id.ToString()));
                }
                comments.Sort((dto, commentDto) => dto.TimeStamp.CompareTo(commentDto.TimeStamp) * -1);
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
