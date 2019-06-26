using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eventcore.Monitoring.SyntheticTransactionService.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Eventcore.Monitoring.SyntheticTransactionService
{
    public class SyntheticTransactionService : IHostedService
    {
        private Task transactionExecutionTask;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            CancellationToken monitoringCancellationToken = Program.CancellationTokenSource.Token;
            transactionExecutionTask = this.ExecuteSyntheticTransactionsAsync(Program.Configuration, monitoringCancellationToken);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Program.CancellationTokenSource.Cancel();
            await Task.WhenAll(this.transactionExecutionTask).ConfigureAwait(false);
        }

        private async Task ExecuteSyntheticTransactionsAsync(IConfiguration configuration, CancellationToken cancellationToken)
        {
            List<ISyntheticTransaction> syntheticTransactions = new List<ISyntheticTransaction>
            {
                new WebsiteAvailabilityCheck()
            };

            Program.Logger.LogInformation(
                $"Executing synthetic transactions:\n{string.Join('\n', syntheticTransactions.Select(trx => trx.GetType().Name))}");

            List<Task> syntheticTransactionTasks = new List<Task>();

            foreach (ISyntheticTransaction transaction in syntheticTransactions)
            {         
                syntheticTransactionTasks.Add(transaction.ExecuteAsync(configuration, cancellationToken));
            }

            await Task.WhenAll(syntheticTransactionTasks).ConfigureAwait(false);
        }
    }
}
