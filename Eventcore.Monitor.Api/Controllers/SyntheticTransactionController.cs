using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eventcore.Monitor.Api.Controllers
{
    /// <summary>
    /// Provides API methods for executing synthetic transactions against target
    /// website endpoints.
    /// </summary>
    [Route("api/monitoring/transactions")]
    [ApiController]
    public class SyntheticTransactionController : ControllerBase
    {
        // POST: api/monitoring/transactions/availability
        [HttpPost("/availability")]
        public void CheckSiteAvailability([FromQuery] Uri uri)
        {

        }

        // POST: api/monitoring/transactions/whatever
        [HttpPost("/whatever")]
        public void CheckSiteWhatever([FromQuery] Uri uri)
        {
        }
    }
}
