using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Eventcore.Telemetry.Data
{
    /// <summary>
    /// Provides methods for reading and writing telemetry event data to
    /// a backing SQL data store.
    /// </summary>
    /// <typeparam name="TContext">The data type of the telemetry event context object.</typeparam>
    public class SqlTelemetryDataStore<TContext> : ITelemetryDataStore<TContext>
    {
        private MySqlConnection sqlConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlTelemetryDataStore{TContext}"/> class.
        /// </summary>
        public SqlTelemetryDataStore(MySqlConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentException("The SQL connection parameter is required.", nameof(connection));
            }

            this.sqlConnection = connection;
        }

        /// <summary>
        /// Creates a new telemetry event item in the backing store.
        /// </summary>
        /// <param name="telemetry">The telemetry event to write to the backing store.</param>
        public async Task CreateDataAsync(TelemetryEvent<TContext> telemetry)
        {
            try
            {
                MySqlCommand command = this.sqlConnection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText =
                    @"INSERT INTO TelemetryEvents
                      (Timestamp, EventName, CorrelationId, Context)
                      VALUES (@Timestamp, @EventName, @CorrelationId, @Context)";

                string eventContext = null;
                if (telemetry.Context != null)
                {
                    eventContext = JsonConvert.SerializeObject(telemetry.Context, TelemetryEvent<TContext>.SerializationSettings);
                }

                command.Parameters.Add(new MySqlParameter("@Timestamp", telemetry.Timestamp));
                command.Parameters.Add(new MySqlParameter("@EventName", telemetry.EventName));
                command.Parameters.Add(new MySqlParameter("@CorrelationId", telemetry.CorrelationId));
                command.Parameters.Add(new MySqlParameter("@Context", eventContext));

                this.sqlConnection.Open();
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
            finally
            {
                this.sqlConnection.Close();
            }
        }

        /// <summary>
        /// Searches for matching telemetry event data in the data store.
        /// </summary>
        /// <param name="filter">Optional filter options to apply to the telemetry event data search.</param>
        public async Task<IEnumerable<TelemetryEvent<TContext>>> SearchDataAsync(FilterOptions filter = null)
        {
            List<TelemetryEvent<TContext>> events = new List<TelemetryEvent<TContext>>();

            try
            {
                MySqlCommand command = this.sqlConnection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText =
                    @"SELECT
                        Timestamp,
                        EventName,
                        CorrelationId,
                        Context
                      FROM TelemetryEvents 
                      WHERE 1";

                if (filter != null && filter.IsSet)
                {
                    if (filter.CorrelationId != null)
                    {
                        command.CommandText += $" AND CorrelationId = '{filter.CorrelationId}'";
                    }

                    if (filter.EventName != null)
                    {
                        command.CommandText += $" AND EventName = '{filter.EventName}'";
                    }

                    if (filter.Latest == true)
                    {
                        if (filter.EventName != null)
                        {
                            command.CommandText +=
                              $@" AND CorrelationId =
                              (
                                  SELECT CorrelationId FROM TelemetryEvents
                                  WHERE EventName = '{filter.EventName}'
                                  ORDER BY Id DESC
                                  LIMIT 1
                              )";
                        }
                        else
                        {
                            command.CommandText +=
                              $@" AND CorrelationId =
                              (
                                  SELECT CorrelationId FROM TelemetryEvents
                                  ORDER BY Id DESC
                                  LIMIT 1
                              )";
                        }
                    }
                }
                else
                {
                    command.CommandText += " LIMIT 1000";
                }

                this.sqlConnection.Open();
                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                while (reader.Read())
                {
                    events.Add(new TelemetryEvent<TContext>(
                        eventName: reader.GetString(1),
                        correlationId: reader.GetGuid(2),
                        timestamp: reader.GetDateTime(0),
                        context: JsonConvert.DeserializeObject<TContext>(reader.GetString(3), TelemetryEvent<TContext>.SerializationSettings)));
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                this.sqlConnection.Close();
            }

            return events;
        }
    }
}
