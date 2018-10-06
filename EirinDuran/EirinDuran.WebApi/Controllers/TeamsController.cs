using System.Collections.Generic;
using System.Linq;
using EirinDuran.IServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EirinDuran.IServices.DTOs;
using System.Security.Claims;
using EirinDuran.IServices.Exceptions;
using System;

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
        public ActionResult<List<TeamDTO>> Get()
        {
            CreateSession();
            try
            {
                return teamServices.GetAllTeams().ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{sportId_teamName}")]
        public ActionResult<TeamDTO> Get(string sportId_teamName)
        {
            try
            {
                return teamServices.GetTeam(sportId_teamName);
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("{sportId_teamName}/encounters")]
        public ActionResult<List<EncounterDTO>> GetEncounters(string sportId_teamName)
        {
            try
            {
                return encounterServices.GetEncountersByTeam(sportId_teamName).ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(TeamDTO team)
        {
            CreateSession();
            try
            {
                return TryToCreate(team);
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
                teamServices.CreateTeam(team);
                return CreatedAtRoute("GetTeam", new { id = team.Name }, team);
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Put(string id, [FromBody] TeamDTO team)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }


        [HttpDelete("{sportId_teamName}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string sportId_teamName)
        {
            try
            {
                return TryToDelete(sportId_teamName);
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToDelete(string sportId_teamName)
        {
            CreateSession();
            try
            {
                teamServices.DeleteTeam(sportId_teamName);
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
