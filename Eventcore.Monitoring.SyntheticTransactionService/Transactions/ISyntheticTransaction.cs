using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Eventcore.Monitoring.SyntheticTransactionService.Transactions
{
    public interface ISyntheticTransaction
    {
        Task ExecuteAsync(IConfiguration configuration, CancellationToken token);
    }
}
