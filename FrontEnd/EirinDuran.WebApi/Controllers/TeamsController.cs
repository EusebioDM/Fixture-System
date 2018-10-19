using System.Collections.Generic;
using System.Linq;
using EirinDuran.IServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EirinDuran.IServices.DTOs;
using System.Security.Claims;
using EirinDuran.IServices.Exceptions;
using System;
using EirinDuran.WebApi.Models;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly ITeamServices teamServices;
        private readonly IEncounterServices encounterServices;

        public TeamsController(ILoginServices loginServices, ITeamServices teamServices, IEncounterServices encounterServices)
        {
            this.loginServices = loginServices;
            this.teamServices = teamServices;
            this.encounterServices = encounterServices;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<TeamModelOut>> Get()
        {
            try
            {
                CreateSession();
                return teamServices.GetAllTeams().Select(t => new TeamModelOut(t)).ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{teamId}", Name = "GetTeam")]
        [Authorize]
        public ActionResult<TeamModelOut> Get(string teamId)
        {
            try
            {
                return new TeamModelOut(teamServices.GetTeam(teamId));
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("{teamId}/encounters")]
        public ActionResult<List<EncounterModelOut>> GetEncounters(string teamId)
        {
            try
            {
                return encounterServices.GetEncountersByTeam(teamId).Select(e => new EncounterModelOut(e)).ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(TeamModelIn team)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                    CreateSession();
                    return TryToCreate(team.Map());
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToCreate(TeamDTO team)
        {
            try
            {
                TeamDTO created = teamServices.CreateTeam(team);
                return CreatedAtRoute("GetTeam", new { teamId = created.Name + "_" + team.SportName } , created);
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Put(string id, [FromBody] TeamModelIn team)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return TryToUpdate(team.Map());
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToUpdate(TeamDTO team)
        {
            try
            {
                teamServices.UpdateTeam(team);
                return Ok();
            }
            catch(ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{teamId}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string teamId)
        {
            try
            {
                return TryToDelete(teamId);
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToDelete(string teamId)
        {
            try
            {
                CreateSession();
                teamServices.DeleteTeam(teamId);
                return Ok();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{teamId}/follower")]
        [Authorize(Roles = "Administrator, Follower")]
        public IActionResult AddFollowedTeamToLogedUser(string teamId)
        {
            try
            {
                CreateSession();
                teamServices.AddFollowedTeam(teamId);
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
