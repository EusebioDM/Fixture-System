using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
    }
}
