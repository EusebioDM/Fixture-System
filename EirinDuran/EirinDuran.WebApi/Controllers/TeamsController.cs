using System.Collections.Generic;
using System.Linq;
using EirinDuran.IServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EirinDuran.IServices.DTOs;
using System.Security.Claims;
using EirinDuran.Domain.Fixture;
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
        private ISportServices sportServices;
        private IUserServices userServices;

        public TeamsController(ILoginServices loginServices, ITeamServices teamServices, ISportServices sportServices, IUserServices userServices)
        {
            this.loginServices = loginServices;
            this.teamServices = teamServices;
            this.sportServices = sportServices;
            this.userServices = userServices;
        }

        [HttpGet]
        public ActionResult<List<TeamDTO>> Get()
        {
            CreateSession();
            userServices.AddFollowedTeam("Boca,Futbol");

            try
            {
                return teamServices.GetAll().ToList();
            }
            catch (ServicesException)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<TeamDTO> Get(string id)
        {
            try
            {
                return teamServices.GetTeam(id);
            }
            catch (ServicesException)
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("{teamId}/encounters")]
        public ActionResult<List<EncounterDTO>> GetEncounters(string teamId)
        {
            try
            {
                return teamServices.GetAllEncounters(new Team(teamId)).ToList();
            }
            catch (ServicesException)
            {
                return BadRequest();
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
            /* 
            Team teamReal = new Team(team.Name
            try
            {
                teamServices.AddTeam(teamReal);
                return CreatedAtRoute("GetTeam", new { id = team.Name }, team);
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
            */
            throw new NotImplementedException();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] TeamDTO team)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string id)
        {
            try
            {
                return TryToDelete(id);
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToDelete(string id)
        {
            CreateSession();
            try
            {
                teamServices.DeleteTeam(id);
                return Ok();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("/follower")]

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
