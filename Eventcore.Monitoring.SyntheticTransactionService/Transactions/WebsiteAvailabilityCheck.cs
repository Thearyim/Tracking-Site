using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eventcore.Telemetry.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eventcore.Monitoring.SyntheticTransactionService.Transactions
{
    public class WebsiteAvailabilityCheck : ISyntheticTransaction
    {
        private Uri telemetryApiUri;
        private TimeSpan runInterval;
        private List<string> siteUrls;

        public async Task ExecuteAsync(IConfiguration configuration, CancellationToken cancellationToken)
        {
            Program.Logger.LogInformation($"{typeof(WebsiteAvailabilityCheck).Name}: Begin execution..." );

            this.InitializeRunSettings(configuration);
            await this.ExecuteSiteAvailabilityChecksAsync(cancellationToken).ConfigureAwait(false);
        }

        private void InitializeRunSettings(IConfiguration configuration)
        {
            JToken transactionConfiguration = JToken.Parse(configuration.GetValue<string>(Constants.SyntheticTransactions));
            this.siteUrls = transactionConfiguration.SelectToken($"{Constants.WebsiteAvailability}.{Constants.SiteUrls}")
                .ToObject<List<string>>();

            this.runInterval = TimeSpan.Parse(
                transactionConfiguration.SelectToken($"{Constants.WebsiteAvailability}.{Constants.RunInterval}").ToString());

            this.telemetryApiUri = new Uri(configuration.GetValue<string>(Constants.TelemetryApiUrl));
        }

        private async Task ExecuteSiteAvailabilityChecksAsync(CancellationToken cancellationToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add(Constants.AcceptHeader, Constants.ApplicationJson);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        Guid correlationId = Guid.NewGuid();
                        foreach (string site in this.siteUrls)
                        {
                            if (!cancellationToken.IsCancellationRequested)
                            {
                                HttpResponseMessage response = await this.ExecuteSiteAvailabilityCheckAsync(httpClient, site, cancellationToken)
                                    .ConfigureAwait(false);

                                await this.WriteTelemetryAsync(httpClient, response, site, correlationId, cancellationToken)
                                    .ConfigureAwait(false);
                            }
                        }

                        Task.Delay(runInterval).GetAwaiter().GetResult();
                        Program.Logger.LogInformation($"{typeof(WebsiteAvailabilityCheck).Name}: Execution complete.");
                    }
                    catch (Exception exc)
                    {
                        // An error occurred while trying to call the site.
                        Program.Logger.LogError($"{typeof(WebsiteAvailabilityCheck).Name} ERROR: {exc.Message}");
                    }
                }
            }
        }

        private async Task<HttpResponseMessage> ExecuteSiteAvailabilityCheckAsync(HttpClient httpClient, string site, CancellationToken cancellationToken)
        {
            Program.Logger.LogInformation($"{typeof(WebsiteAvailabilityCheck).Name}: Probing {site}");

            HttpResponseMessage response = await httpClient.GetAsync(site, cancellationToken)
                .ConfigureAwait(false);

            Program.Logger.LogInformation($"{typeof(WebsiteAvailabilityCheck).Name}: Site Response ({(int)response.StatusCode} - {response.StatusCode.ToString()})");
            return response;            
        }

        private async Task WriteTelemetryAsync(HttpClient httpClient, HttpResponseMessage siteResponse, string site, Guid correlationId, CancellationToken cancellationToken)
        {
            TelemetryEvent<IDictionary<string, object>> telemetry = new TelemetryEvent<IDictionary<string, object>>(
                "WebsiteAvailabilityCheck",
                correlationId,
                DateTime.UtcNow,
                new Dictionary<string, object>
                {
                    ["url"] = site,
                    ["status"] = siteResponse.StatusCode.ToString(),
                    ["statusCode"] = (int)siteResponse.StatusCode
                });

            string serializedTelemetry = JsonConvert.SerializeObject(
                telemetry,
                TelemetryEvent<IDictionary<string, object>>.SerializationSettings);

            HttpResponseMessage apiResponse = await httpClient.PostAsync(
                telemetryApiUri,
                new StringContent(serializedTelemetry, Encoding.UTF8, Constants.ApplicationJson))
                .ConfigureAwait(false);

            if (!apiResponse.IsSuccessStatusCode)
            {
                Program.Logger.LogError(
                    $"{typeof(WebsiteAvailabilityCheck).Name} ERROR: Telemetry API response ({(int)apiResponse.StatusCode} - {apiResponse.StatusCode.ToString()}, " +
                    $"{await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false)})");
            }
        }
    }
}
