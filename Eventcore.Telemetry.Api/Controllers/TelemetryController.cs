using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventcore.Telemetry.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Eventcore.Telemetry.Api.Controllers
{
    /// <summary>
    /// Eventcore telemetry API controller.
    /// </summary>
    [Route("api/telemetry")]
    [ApiController]
    public class TelemetryController : ControllerBase
    {
        public TelemetryController(ITelemetryDataStore<IDictionary<string, object>> dataStore)
        {
            this.DataStore = dataStore
                ?? throw new ArgumentException("The data store parameter is required.", nameof(dataStore));
        }

        /// <summary>
        /// Gets the data store.
        /// </summary>
        protected ITelemetryDataStore<IDictionary<string, object>> DataStore { get; }

        // GET api/telemetry?eventName=WebsiteAvailabilityCheck&latest=true
        [HttpGet]
        public async Task<IActionResult> GetEventsAsync(string eventName, Guid? correlationId, bool? latest)
        {
            IActionResult result = null;

            try
            {
                this.AddCrossOriginHeaders();

                FilterOptions filter = new FilterOptions(eventName, correlationId, latest);
                IEnumerable<TelemetryEvent<IDictionary<string, object>>> events = await this.DataStore.SearchDataAsync(filter)
                    .ConfigureAwait(false);

                result = this.Ok(events);
            }
            catch (Exception exc)
            {
                result = new ObjectResult(exc)
                {
                    StatusCode = 500
                };
            }

            return result;
        }

        // POST api/telemetry
        //
        // [HTTP Body]
        // {
        //    'eventName': 'WebsiteAvailabilityCheck',
        //    'correlationId': 'e915e981-92ae-4968-84c1-52437f3c265a',
        //    'timestamp' : '2019-06-11T00:00:00.0000000Z',
        //    'context': {
        //      'name' : 'Eventcore',
        //      'url' : 'https://www.eventcore.com/',
        //      'status' : 'OK',
        //      'statusCode': 200
        //    }
        // }
        [HttpPost]
        public async Task<IActionResult> WriteEventAsync([FromBody] TelemetryEvent<IDictionary<string, object>> telemetryEvent)
        {
            IActionResult result = null;

            try
            {
                this.AddCrossOriginHeaders();

                await this.DataStore.CreateDataAsync(telemetryEvent).ConfigureAwait(false);
                result = this.Ok();
            }
            catch (Exception exc)
            {
                result = new ObjectResult(exc)
                {
                    StatusCode = 500
                };
            }

            return result;
        }

        private void AddCrossOriginHeaders()
        {
            const string allowOriginHeader = "Access-Control-Allow-Origin";
            const string allowMethodsHeader = "Access-Control-Allow-Methods";
            const string allowHeadersHeader = "Access-Control-Allow-Headers";

            if (!this.Response.Headers.ContainsKey(allowOriginHeader))
            {
                this.Response.Headers.Add(allowOriginHeader, new StringValues("http://localhost:8080"));
            }

            if (!this.Response.Headers.ContainsKey(allowMethodsHeader))
            {
                this.Response.Headers.Add(allowMethodsHeader, new StringValues(new string[] { "GET", "POST" }));
            }

            if (!this.Response.Headers.ContainsKey(allowHeadersHeader))
            {
                this.Response.Headers.Add(allowHeadersHeader, new StringValues("Content-Type"));
            }
        }
    }
}
