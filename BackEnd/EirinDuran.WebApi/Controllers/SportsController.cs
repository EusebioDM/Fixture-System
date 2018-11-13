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
    public class SportsController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly ISportServices sportServices;
        private readonly IEncounterSimpleServices encounterSimpleServices;
        private readonly IEncounterQueryServices encounterQueryServices;
        private readonly IPositionsServices positionsServices;

        public SportsController(ILoginServices loginServices, ISportServices sportServices, IEncounterSimpleServices encounterSimpleServices, IEncounterQueryServices encounterQueryServices, IPositionsServices positionsServices)
        {
            this.loginServices = loginServices;
            this.sportServices = sportServices;
            this.encounterSimpleServices = encounterSimpleServices;
            this.encounterQueryServices = encounterQueryServices;
            this.positionsServices = positionsServices;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Follower")]
        public ActionResult<List<SportModelOut>> GetAll()
        {
            try
            {
                return sportServices.GetAllSports().Select(s => new SportModelOut(s)).ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("{sportId}", Name = "GetSport")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<SportModelOut> GetById(string sportId)
        {
            try
            {
                return new SportModelOut(sportServices.GetSport(sportId));
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{sportId}/results", Name = "GetSportTable")]
        [Authorize]
        public ActionResult<Dictionary<string, int>> GetPositionsTable(string sportId)
        {
            try
            {
                return positionsServices.GetPositionsTable(new SportDTO() {Name = sportId});
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("{sportId}/encounters")]
        [Authorize]
        public ActionResult<List<EncounterModelOut>> GetEncounters(string sportId)
        {
            try
            {
                return encounterQueryServices.GetEncountersBySport(sportId).Select(e => new EncounterModelOut(e)).ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(SportDTO sport)
        {
            try
            {
                CreateSession();
                return TryToCreate(sport);
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

        private IActionResult TryToCreate(SportDTO sport)
        {
            sportServices.CreateSport(sport);
            return CreatedAtRoute("GetSport", new { sportId = sport.Name }, sport);
        }

        [HttpDelete("{sportName}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string sportName)
        {
            
            try
            {
                CreateSession();
                return TryToDelete(sportName);
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

        private IActionResult TryToDelete(string sportId)
        {
            sportServices.DeleteSport(sportId);
            return Ok();
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