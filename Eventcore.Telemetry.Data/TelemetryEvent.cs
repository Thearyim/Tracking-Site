using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Eventcore.Telemetry.Data
{
    /// <summary>
    /// Represents the details of a telemetry event.
    /// </summary>
    /// <typeparam name="TContext">The data type of the telemetry context object.</typeparam>
    public class TelemetryEvent<TContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryEvent{TData}"/> class.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="correlationId">The unique identifier for the event.</param>
        /// <param name="timestamp">The date at which the event occurred.</param>
        /// <param name="context">Context data/information associated with the event.</param>
        public TelemetryEvent(string eventName, Guid correlationId, DateTime timestamp, TContext context)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentException("The item event name parameter must be defined", nameof(eventName));
            }

            this.EventName = eventName;
            this.CorrelationId = correlationId;
            this.Timestamp = timestamp;
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the JSON serialization settings to use when serializing/deserializing
        /// the telemetry context objects.
        /// </summary>
        public static JsonSerializerSettings SerializationSettings { get; set; } = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        /// <summary>
        /// Gets the name of the telemetry event.
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// Gets the telemetry event correlation id.
        /// </summary>
        public Guid CorrelationId { get; }

        /// <summary>
        /// Gets the date/time at which the telemetry event happened.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the the object representing the context of the
        /// telemetry event.
        /// </summary>
        public TContext Context { get; }
    }
}
