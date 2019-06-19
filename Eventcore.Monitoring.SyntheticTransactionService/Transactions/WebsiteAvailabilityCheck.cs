using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Eventcore.Telemetry.Api.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Eventcore.Monitoring.SyntheticTransactionService.Transactions
{
    public class WebsiteAvailabilityCheck : ISyntheticTransaction
    {
        private const string SyntheticTransactionsSection = "SyntheticTransactions";
        private const string WebsiteAvailabilitySection = "WebsiteAvailability";
        private const string SiteUrls = "SiteUrls";

        public async Task ExecuteAsync(IConfiguration configuration, CancellationToken cancellationToken)
        {
            IEnumerable<string> siteUrls = configuration
                .GetSection(WebsiteAvailabilityCheck.SyntheticTransactionsSection)
                .GetSection(WebsiteAvailabilityCheck.WebsiteAvailabilitySection)
                .GetSection(WebsiteAvailabilityCheck.SiteUrls)
                .Get<List<string>>();

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept-Content", "application/json");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Guid correlationId = Guid.NewGuid();
                    Console.WriteLine($"{typeof(WebsiteAvailabilityCheck).Name}: Executing synthetic transaction...");
                    foreach (string site in siteUrls)
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            Console.WriteLine($"{typeof(WebsiteAvailabilityCheck).Name}: Calling '{site}'");
                            HttpResponseMessage response = await httpClient.GetAsync(site, cancellationToken)
                             .ConfigureAwait(false);

                            TelemetryEvent<IDictionary<string, object>> telemetry = new TelemetryEvent<IDictionary<string, object>>(
                                "WebsiteAvailabilityCheck",
                                correlationId,
                                DateTime.UtcNow,
                                new Dictionary<string, object>
                                {
                                    ["url"] = site,
                                    ["status"] = (int)response.StatusCode
                                });

                            await httpClient.PostAsync("http://localhost:5000/api/telemetry", new StringContent(
                                    JsonConvert.SerializeObject(telemetry))).ConfigureAwait(false);
                        }
                    }

                    Task.Delay(3000).GetAwaiter().GetResult();
                }
                catch (Exception exc)
                {
                    // An error occurred while trying to call the site.
                }
            }
        }
    }
}
