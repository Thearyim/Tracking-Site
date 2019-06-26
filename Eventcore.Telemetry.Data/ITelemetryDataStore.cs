using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventcore.Telemetry.Data
{
    /// <summary>
    /// Provides methods for reading and writing telemetry data to a backing
    /// data store.
    /// </summary>
    public interface ITelemetryDataStore<TContext>
    {
        /// <summary>
        /// Creates an item in the backing data store.
        /// </summary>
        /// <param name="telemetry">The telemetry event to create/save.</param>
        Task CreateDataAsync(TelemetryEvent<TContext> telemetry);

        /// <summary>
        /// Searches for matching telemetry event data in the data store.
        /// </summary>
        /// <param name="filter">Optional filter options to apply to the telemetry event data search.</param>
        Task<IEnumerable<TelemetryEvent<TContext>>> SearchDataAsync(FilterOptions filter = null);        
    }
}
