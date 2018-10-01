using System.Collections.Generic;
using System.Linq;
using EirinDuran.IServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EirinDuran.IServices.DTOs;
using System.Security.Claims;
using EirinDuran.Domain.Fixture;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly ITeamServices teamServices;

        public TeamsController(ILoginServices loginServices, ITeamServices teamServices)
        {
            this.loginServices = loginServices;
            this.teamServices = teamServices;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
            catch(InsufficientPermissionException)
            {
                return Unauthorized();
            }            
        }

        private IActionResult TryToCreate(TeamDTO team)
        {
            Team teamReal = new Team(team.Name);
            try
            {
                teamServices.AddTeam(teamReal);
                return CreatedAtRoute("GetTeam", new { id = team.Name }, team);
            }
            catch (ServicesException)
            {
                return BadRequest();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
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
            try
            {
                teamServices.DeleteTeam(id);
                return Ok();
            }
            catch (ServicesException)
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
