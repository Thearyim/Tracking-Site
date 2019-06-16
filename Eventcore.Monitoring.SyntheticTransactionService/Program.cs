using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Eventcore.Monitoring.SyntheticTransactionService
{
    class Program
    {
        internal static IConfiguration Configuration;
        internal static ILogger Logger;
        internal static CancellationTokenSource CancellationTokenSource;

        /// <summary>
        /// Entry point for the Synthetic Transaction Service.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                Program.CancellationTokenSource = new CancellationTokenSource();
                Program.SetupLogging();
                Program.Logger.LogInformation("Synthetic Transaction Service starting up.");
                IHost applicationHost = Program.CreateHost(args);

                Task hostRunningTask = applicationHost.RunAsync(CancellationToken.None);
                Task.WhenAll(hostRunningTask).GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                // Expected when the Ctrl-C is pressed to cancel operation.
            }
            catch (Exception exc)
            {
                Program.Logger.LogError(exc.Message);
                throw;
            }
        }

        private static IHost CreateHost(string[] args)
        {
            return new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    // Add configuration settings defined in *.appsettings.json file.
                    IConfiguration hostConfiguration = new ConfigurationBuilder()
                        .AddJsonFile(
                            Path.Combine(
                                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                $"appsettings.json"))
                        .Build();

                    Program.Configuration = hostConfiguration;
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<SyntheticTransactionService>();
                })
                .UseConsoleLifetime()
                .Build();
        }

        private static void SetupLogging()
        {
            //LoggerFactory loggerFactory = new LoggerFactory();
            //loggerFactory.AddProvider(new Conso)
            Program.Logger = NullLogger.Instance;
        }
    }
}
