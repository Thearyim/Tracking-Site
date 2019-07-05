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

namespace Eventcore.SyntheticTransactionService.Transactions
{
    public class WebsiteAvailabilityCheck : ISyntheticTransaction
    {
        private Uri telemetryApiUri;
        private TimeSpan runInterval;
        private List<Dictionary<string, string>> sites;

        public async Task ExecuteAsync(IConfiguration configuration, CancellationToken cancellationToken)
        {
            Program.Logger.LogInformation($"{typeof(WebsiteAvailabilityCheck).Name}: Begin execution..." );

            this.InitializeRunSettings(configuration);
            await this.ExecuteSiteAvailabilityChecksAsync(cancellationToken).ConfigureAwait(false);
        }

        private void InitializeRunSettings(IConfiguration configuration)
        {
            JToken transactionConfiguration = JToken.Parse(configuration.GetValue<string>(Constants.SyntheticTransactions));
            this.sites = transactionConfiguration.SelectToken($"{Constants.WebsiteAvailability}.{Constants.Sites}")
                .ToObject<List<Dictionary<string, string>>>();

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
                        foreach (IDictionary<string, string> site in this.sites)
                        {
                            if (!cancellationToken.IsCancellationRequested)
                            {
                                HttpResponseMessage response = null;

                                string siteName = site["name"];
                                string siteUrl = site["url"];

                                try
                                {
                                    response = await this.ExecuteSiteAvailabilityCheckAsync(httpClient, siteUrl, cancellationToken)
                                        .ConfigureAwait(false);
                                }
                                catch (HttpRequestException exc)
                                {
                                    Program.Logger.LogError($"{typeof(WebsiteAvailabilityCheck).Name} REQUEST ERROR: {exc.Message}");
                                    response = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                                }

                                await this.WriteTelemetryAsync(httpClient, response, siteName, siteUrl, correlationId, cancellationToken)
                                    .ConfigureAwait(false);
                            }
                        }

                        Program.Logger.LogInformation($"{typeof(WebsiteAvailabilityCheck).Name}: Execution complete.");
                    }
                    catch (Exception exc)
                    {
                        // An error occurred while trying to call the site.
                        Program.Logger.LogError($"{typeof(WebsiteAvailabilityCheck).Name} ERROR: {exc.Message}");
                    }
                    finally
                    {
                        Task.Delay(runInterval, cancellationToken).GetAwaiter().GetResult();
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

        private async Task WriteTelemetryAsync(HttpClient httpClient, HttpResponseMessage siteResponse, string siteName, string siteUrl, Guid correlationId, CancellationToken cancellationToken)
        {
            TelemetryEvent<IDictionary<string, object>> telemetry = new TelemetryEvent<IDictionary<string, object>>(
                "WebsiteAvailabilityCheck",
                correlationId,
                DateTime.UtcNow,
                new Dictionary<string, object>
                {
                    ["name"] = siteName,
                    ["url"] = siteUrl,
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
