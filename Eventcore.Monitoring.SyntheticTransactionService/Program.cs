using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            }
        }

        private static IHost CreateHost(string[] args)
        {
            return new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    Program.Logger.LogInformation("Creating synthetic transaction service host...");

                    // Add configuration settings defined in *.appsettings.json file.
                    IConfiguration hostConfiguration = new ConfigurationBuilder()
                        .AddJsonFile(
                            Path.Combine(
                                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                $"appsettings.json"))              
                        .Build();

                    IConfiguration syntheticTransactionConfiguration = Program.GetSyntheticTransactionConfiguration(hostConfiguration);

                    Program.Configuration = new ConfigurationBuilder()
                        .AddConfiguration(hostConfiguration)
                        .AddConfiguration(syntheticTransactionConfiguration)
                        .Build();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<SyntheticTransactionService>();
                })
                .UseConsoleLifetime()
                .Build();
        }

        private static IConfiguration GetSyntheticTransactionConfiguration(IConfiguration hostConfiguration)
        {
            IConfiguration configuration = null;

            string environment = hostConfiguration.GetValue<string>(Constants.ConfigurationEnvironment);
            Uri configurationApiUri = new Uri($"{hostConfiguration.GetValue<string>(Constants.ConfigurationApiUrl)}/{environment}");

            Program.Logger.LogInformation($"Loading synthetic transaction configuration settings.");
            Program.Logger.LogInformation($"Configuration API: {configurationApiUri}");

            using (HttpClient configurationClient = new HttpClient())
            {
                HttpResponseMessage response = configurationClient.GetAsync(configurationApiUri)
                    .GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = response.Content.ReadAsStringAsync()
                        .GetAwaiter().GetResult();

                    IDictionary<string, string> settings = new Dictionary<string, string>
                    {
                        [Constants.SyntheticTransactions] = JToken.Parse(jsonContent).SelectToken(Constants.SyntheticTransactions).ToString()
                    };

                    configuration = new ConfigurationBuilder()
                        .AddInMemoryCollection(settings)
                        .Build();
                }
            }

            return configuration;
        }

        private static void SetupLogging()
        {
            //LoggerFactory loggerFactory = new LoggerFactory();
            //loggerFactory.AddProvider(new Conso)
            LoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();
            Program.Logger = loggerFactory.CreateLogger("SyntheticTransactions");
        }
    }
}
