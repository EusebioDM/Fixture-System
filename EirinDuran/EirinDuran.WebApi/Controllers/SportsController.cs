using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly ISportServices sportServices;

        public SportsController(ILoginServices loginServices, ISportServices sportServices)
        {
            this.loginServices = loginServices;
            this.sportServices = sportServices;
        }

        [HttpGet]
        public ActionResult<List<SportDTO>> Get()
        {
            return sportServices.GetAllSports().ToList();
        }

        [HttpGet("{id}", Name = "GetSport")]
        public ActionResult<SportDTO> GetById(string id)
        {
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(SportDTO sport)
        {
            CreateSession();
            sportServices.Create(sport);
            return CreatedAtRoute("GetSport", new { id = sport.Name }, sport);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
