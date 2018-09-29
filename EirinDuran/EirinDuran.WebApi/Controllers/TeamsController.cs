using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services;
using Microsoft.AspNetCore.Mvc;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly TeamServices teamServices;

        public TeamsController(ILoginServices loginServices, TeamServices teamServices)
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
