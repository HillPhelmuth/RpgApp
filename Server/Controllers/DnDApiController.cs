using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RpgApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DnDApiController : ControllerBase
    {
        // GET: api/<RpgController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/<RpgController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RpgController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RpgController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RpgController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
