using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Eventcore.Telemetry.Data
{
    /// <summary>
    /// Provides methods for reading, writing and managing monitoring data
    /// in a file system-based data store.
    /// </summary>
    /// <typeparam name="TContext">The data type of the telemetry event context object.</typeparam>
    public class FileSystemTelemetryDataStore<TContext> : ITelemetryDataStore<TContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemTelemetryDataStore{TData}"/> class.
        /// </summary>
        public FileSystemTelemetryDataStore(DirectoryInfo storageDirectory)
        {
            if (storageDirectory == null)
            {
                throw new ArgumentException("The storage directory parameter is required.", nameof(storageDirectory));
            }

            this.StorageDirectory = storageDirectory;
        }

        /// <summary>
        /// Gets the parent storage directory where the data will be stored.
        /// </summary>
        protected DirectoryInfo StorageDirectory { get; }

        /// <inheritdoc />
        public Task CreateDataAsync(TelemetryEvent<TContext> telemetry)
        {
            return Task.Run(() =>
            {
                string filePath = this.GetFilePath(telemetry.EventName, telemetry.CorrelationId);
                string fileDirectory = Path.GetDirectoryName(filePath);
                string fileContent = FileSystemTelemetryDataStore<TContext>.SerializeItem(telemetry);

                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }

                File.WriteAllText(filePath, fileContent);
            });
        }

        /// <inheritdoc />
        public Task<IEnumerable<TelemetryEvent<TContext>>> SearchDataAsync(FilterOptions filter = null)
        {
            return Task.Run(() =>
            {
                List<TelemetryEvent<TContext>> matchingEvents = new List<TelemetryEvent<TContext>>();

                if (filter.EventName == null && filter.CorrelationId != null)
                {
                    throw new NotSupportedException(
                        "When a correlation ID is provided in the filter, an event name must be provided as well.");
                }

                FileInfo[] matchingFiles = null;
                DirectoryInfo matchingDirectory = new DirectoryInfo(this.GetFilePath(filter));
                if (matchingDirectory.Exists)
                {
                    if (filter.CorrelationId != null)
                    {
                        // A given correlation ID corresponds to a single directory only.
                        matchingFiles = matchingDirectory.GetFiles("*.json", SearchOption.TopDirectoryOnly);
                    }
                    else if (filter.Latest == true)
                    {
                        DirectoryInfo latestDirectory = matchingDirectory.GetDirectories()?.OrderByDescending(dir => dir.CreationTime)
                            .FirstOrDefault();

                        matchingFiles = latestDirectory?.GetFiles("*.json");
                    }
                    else
                    {
                        matchingFiles = matchingDirectory.GetFiles("*.json", SearchOption.AllDirectories);
                    }

                    if (matchingFiles != null)
                    {
                        foreach (FileInfo file in matchingFiles)
                        {
                            string fileContent = File.ReadAllText(file.FullName);
                            matchingEvents.Add(FileSystemTelemetryDataStore<TContext>.DeserializeItem(fileContent));
                        }
                    }
                }

                return matchingEvents as IEnumerable<TelemetryEvent<TContext>>;
            });
        }

        private static TelemetryEvent<TContext> DeserializeItem(string itemContent)
        {
            return JsonConvert.DeserializeObject<TelemetryEvent<TContext>>(
                itemContent,
                TelemetryEvent<TContext>.SerializationSettings);
        }

        private static string SerializeItem(TelemetryEvent<TContext> item)
        {
            return JsonConvert.SerializeObject(item, TelemetryEvent<TContext>.SerializationSettings);
        }

        private string GetFilePath(FilterOptions filter)
        {
            string filePath = this.StorageDirectory.FullName;
            if (filter.EventName != null)
            {
                filePath = Path.Combine(filePath, filter.EventName);
                if (filter.CorrelationId != null)
                {
                    filePath = Path.Combine(filePath, filter.CorrelationId.ToString());
                }
            }

            return filePath;
        }

        private string GetFilePath(string eventName, Guid correlation)
        {
            // Structure Format:
            // C:\....\<eventName>\<correlationId>\<randomGuid>.json
            //
            // Example Folder Structure:
            // C:\eventcore\monitoring\WebsiteAvailabilityCheck\FC7BB44F-36A5-484F-AEA2-9A338EA5555B\E8EE8ECE-B442-47AC-B447-9DBDBDF69949.json
            // C:\eventcore\monitoring\WebsiteAvailabilityCheck\FC7BB44F-36A5-484F-AEA2-9A338EA5555B\BE06895A-7FAA-4561-919C-3405212859A5.json
            return Path.Combine(this.StorageDirectory.FullName, $@"{eventName}\{correlation}\{Guid.NewGuid()}.json");
        }
    }
}
