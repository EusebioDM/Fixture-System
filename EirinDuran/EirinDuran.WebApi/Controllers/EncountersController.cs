using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EirinDuran.Domain.Fixture;
using EirinDuran.IServices.Interfaces;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncountersController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly IEncounterServices encounterServices;

        public EncountersController(ILoginServices loginServices, IEncounterServices encounterServices)
        {
            this.encounterServices = encounterServices;
            this.loginServices = loginServices;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<Encounter>> Get()
        {
            CreateSession();
            return encounterServices.GetAllEncounters().ToList();
        }

        [HttpGet("{id}", Name = "GetEncounter")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<EncounterDTO> GetById(string id)
        {
            CreateSession();
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(EncounterDTO encounter)
        {
            CreateSession();
            if (ModelState.IsValid)
            {
                return TryToAddEncounter(encounter);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private IActionResult TryToAddEncounter(EncounterDTO encounter)
        {
            try
            {
                encounterServices.CreateEncounter(encounter);
                return CreatedAtRoute("GetEncounter", new { id = encounter.Id }, encounter);
            }
            catch (InsufficientPermissionException)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Put(string id, [FromBody] EncounterDTO encounterModel)
        {
            CreateSession();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string id)
        {
            CreateSession();
            return TryToDelete(id);
        }

        private IActionResult TryToDelete(string id)
        {
            return BadRequest();
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
