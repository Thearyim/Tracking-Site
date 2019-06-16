using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Eventcore.Monitoring.SyntheticTransactionService.Transactions
{
    public class WebsiteAvailabilityCheck : ISyntheticTransaction
    {
        private const string SyntheticTransactionsSection = "SyntheticTransactions";
        private const string WebsiteAvailabilitySection = "WebsiteAvailability";
        private const string SiteUrls = "SiteUrls";

        public Task ExecuteAsync(IConfiguration configuration, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                IEnumerable<string> siteUrls = configuration
                    .GetSection(WebsiteAvailabilityCheck.SyntheticTransactionsSection)
                    .GetSection(WebsiteAvailabilityCheck.WebsiteAvailabilitySection)
                    .GetSection(WebsiteAvailabilityCheck.SiteUrls)
                    .Get<List<string>>();

                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"{typeof(WebsiteAvailabilityCheck).Name}: Executing synthetic transaction...");
                    foreach (string site in siteUrls)
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            Console.WriteLine($"{typeof(WebsiteAvailabilityCheck).Name}: Calling '{site}'");
                            Task.Delay(3000).GetAwaiter().GetResult();
                        }
                    }

                    Task.Delay(3000).GetAwaiter().GetResult();
                }
            });
        }
    }
}
