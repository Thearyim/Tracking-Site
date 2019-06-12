using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aguacongas.Firebase;
using Microsoft.AspNetCore.Mvc;

namespace Eventcore.Monitor.Api.Controllers
{
    [Route("api/monitoring/telemetry")]
    [ApiController]
    public class TelemetryController : ControllerBase
    {
        // https://www.nuget.org/packages/Aguacongas.Firebase/
        // https://github.com/aguacongas/Identity.Firebase
        private FirebaseClient firebaseClient;

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
            
            // "service1" : 
            // {
            //    "id" : "4042B3FD-3EFF-4238-9BCF-E15BCB4B3C89",
            //    "lastChecked" : "2019-06-11T00:00:00.0000000Z",
            //    "status" : 200,
            //    "uri" : "https://www.eventcore.com/"
            // }
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
