using System;
using Newtonsoft.Json;

namespace Eventcore.Telemetry.Data
{
    /// <summary>
    /// Provides search filter options to apply to telemetry data
    /// searches.
    /// </summary>
    public class FilterOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterOptions"/> class.
        /// </summary>
        public FilterOptions(string eventName, Guid? correlationId, bool? latest)
        {
            this.EventName = eventName;
            this.CorrelationId = correlationId;
            this.Latest = latest;
        }

        /// <summary>
        /// Gets or sets the event name on which to filter.
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// Gets or sets the event correlation ID on which to filter.
        /// </summary>
        public Guid? CorrelationId { get; }

        /// <summary>
        /// Gets or sets true/false whether to filter for the latest.
        /// </summary>
        public bool? Latest { get; }

        /// <summary>
        /// Gets true/false whether at least one filter option is set.
        /// </summary>
        [JsonIgnore]
        public bool IsSet
        {
            get
            {
                return this.EventName != null || this.CorrelationId != null || this.Latest != null;
            }
        }
    }
}
