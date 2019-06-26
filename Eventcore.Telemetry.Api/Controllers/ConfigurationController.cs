using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Eventcore.Telemetry.Api.Controllers
{
    /// <summary>
    /// Eventcore configuration API controller.
    /// </summary>
    [Route("api/configuration")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        public ConfigurationController(DirectoryInfo storageDirectory)
        {
            this.StorageDirectory = storageDirectory
                ?? throw new ArgumentException("The storage directory parameter is required.", nameof(storageDirectory));
        }

        /// <summary>
        /// Gets the directory where the configuration documents/files will
        /// be stored.
        /// </summary>
        protected DirectoryInfo StorageDirectory { get; }

        // GET api/configuration/production
        [HttpGet("{configurationId}")]
        public Task<IActionResult> GetConfigurationAsync(string configurationId)
        {
            return Task.Run(() =>
            {
                IActionResult result = null;

                try
                {
                    string path = this.GetStoragePath(configurationId);
                    if (!System.IO.File.Exists(path))
                    {
                        result = this.NotFound();
                    }
                    else
                    {
                        string configuration = System.IO.File.ReadAllText(path);
                        result = this.Ok(configuration);
                    }
                }
                catch (Exception exc)
                {
                    result = new ObjectResult(exc)
                    {
                        StatusCode = 500
                    };
                }

                return result;
            });
        }

        // PUT api/configuration
        //
        // [HTTP Body]
        // {
        //  "id": "production",
        //  "syntheticTransactions": {
        //    "websiteAvailability": {
        //      "siteUrls": [
        //        "www.eventcore.com",
        //        "www.google.com"
        //      ]
        //    }
        //  }
        //}
        [HttpPut]
        public Task<IActionResult> WriteConfigurationAsync([FromBody]JObject configuration)
        {
            return Task.Run(() =>
            {
                IActionResult result = null;

                try
                {
                    JToken configurationId = configuration.GetValue("Id");
                    if (configurationId == null)
                    {
                        result = this.BadRequest("A configuration ID is required. The JSON document provided must contain an 'Id' property.");
                    }
                    else
                    {
                        string path = this.GetStoragePath(configurationId.ToString());
                        if (!this.StorageDirectory.Exists)
                        {
                            this.StorageDirectory.Create();
                        }

                        System.IO.File.WriteAllText(path, configuration.ToString());
                        result = this.Ok(configuration);
                    }
                }
                catch (Exception exc)
                {
                    result = new ObjectResult(exc)
                    {
                        StatusCode = 500
                    };
                }

                return result;
            });
        }

        private string GetStoragePath(string configurationId)
        {
            return Path.Combine(this.StorageDirectory.FullName, $"{configurationId}.json");
        }
    }
}
