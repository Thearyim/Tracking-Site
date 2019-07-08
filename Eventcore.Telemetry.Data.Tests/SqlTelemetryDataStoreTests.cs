using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MySql.Data.MySqlClient;

namespace Eventcore.Telemetry.Data.UnitTests
{
    [TestClass]
    public class SqlTelemetryDataStoreTests
    {
        private TestSqlTelemetryDataStore dataStore;
        private Mock<IDbConnection> mockConnection;
        private Mock<IDbCommand> mockCommand;
        private Mock<IDataParameterCollection> mockCommandParameters;

        [TestInitialize]
        public void SetupTest()
        {
            this.dataStore = new TestSqlTelemetryDataStore("Any");
            this.mockConnection = new Mock<IDbConnection>();
            this.mockCommand = new Mock<IDbCommand>();
            this.mockCommandParameters = new Mock<IDataParameterCollection>();
        }

        [TestMethod]
        public void DataStoreExecutesTheExpectedCommandToWriteTelemetryEventsToTheDatabase()
        {
            TestSqlTelemetryDataStore dataStore = new TestSqlTelemetryDataStore("Any connection string");

            MySqlConnection connection = new MySqlConnection("Server=AnyServer;Database=AnyDatabase;Port=1234");
            MySqlCommand command = dataStore.CreateTelemetryInsertCommand(
                connection,
                new TelemetryEvent<string>("AnyEvent", Guid.NewGuid(), DateTime.Now, "AnyContext"));

            Assert.AreEqual(
                @"INSERT INTO TelemetryEvents
                (Timestamp, EventName, CorrelationId, Context)
                VALUES (@Timestamp, @EventName, @CorrelationId, @Context)",
                command.CommandText);

            Assert.AreEqual(CommandType.Text, command.CommandType);
            Assert.AreEqual(4, command.Parameters.Count);
            Assert.IsTrue(command.Parameters.Contains("@Timestamp"));
            Assert.IsTrue(command.Parameters.Contains("@EventName"));
            Assert.IsTrue(command.Parameters.Contains("@CorrelationId"));
            Assert.IsTrue(command.Parameters.Contains("@Context"));
        }

        [TestMethod]
        public void DataStoreExecutesTheExpectedCommandToSearchTelemetryEventsInTheDatabaseWhenAFilterIsNotProvided()
        {
            TestSqlTelemetryDataStore dataStore = new TestSqlTelemetryDataStore("Any connection string");

            MySqlConnection connection = new MySqlConnection("Server=AnyServer;Database=AnyDatabase;Port=1234");
            MySqlCommand command = dataStore.CreateTelemetrySearchCommand(connection, null);

            Assert.AreEqual(
                RemoveWhitespace(
                    @"SELECT
                    Timestamp,
                    EventName,
                    CorrelationId,
                    Context
                    FROM TelemetryEvents 
                    WHERE 1 ORDER BY TIMESTAMP DESC LIMIT 300"),
                RemoveWhitespace(command.CommandText));

            Assert.AreEqual(CommandType.Text, command.CommandType);
            Assert.AreEqual(0, command.Parameters.Count);
        }

        [TestMethod]
        public void DataStoreExecutesTheExpectedCommandToSearchTelemetryEventsInTheDatabaseWhenAFilterIsProvided1()
        {
            TestSqlTelemetryDataStore dataStore = new TestSqlTelemetryDataStore("Any connection string");

            MySqlConnection connection = new MySqlConnection("Server=AnyServer;Database=AnyDatabase;Port=1234");
            MySqlCommand command = dataStore.CreateTelemetrySearchCommand(connection, new FilterOptions("AnyEventName", null, null));

            Assert.AreEqual(
                RemoveWhitespace(
                    @"SELECT
                    Timestamp,
                    EventName,
                    CorrelationId,
                    Context
                    FROM TelemetryEvents 
                    WHERE 1 AND EventName = 'AnyEventName'"),
                RemoveWhitespace(command.CommandText));

            Assert.AreEqual(CommandType.Text, command.CommandType);
            Assert.AreEqual(0, command.Parameters.Count);
        }

        [TestMethod]
        public void DataStoreExecutesTheExpectedCommandToSearchTelemetryEventsInTheDatabaseWhenAFilterIsProvided2()
        {
            TestSqlTelemetryDataStore dataStore = new TestSqlTelemetryDataStore("Any connection string");

            MySqlConnection connection = new MySqlConnection("Server=AnyServer;Database=AnyDatabase;Port=1234");
            MySqlCommand command = dataStore.CreateTelemetrySearchCommand(connection, new FilterOptions("AnyEventName", null, true));

            Assert.AreEqual(
                RemoveWhitespace(
                    @"SELECT
                    Timestamp,
                    EventName,
                    CorrelationId,
                    Context
                    FROM TelemetryEvents 
                    WHERE 1 AND EventName = 'AnyEventName' AND CorrelationId =
                    (
                        SELECT CorrelationId FROM TelemetryEvents
                        WHERE EventName = 'AnyEventName'
                        ORDER BY Id DESC
                        LIMIT 1
                    ) ORDER BY TIMESTAMP DESC"),
                RemoveWhitespace(command.CommandText));

            Assert.AreEqual(CommandType.Text, command.CommandType);
            Assert.AreEqual(0, command.Parameters.Count);
        }

        [TestMethod]
        public void DataStoreExecutesTheExpectedCommandToSearchTelemetryEventsInTheDatabaseWhenAFilterIsProvided3()
        {
            TestSqlTelemetryDataStore dataStore = new TestSqlTelemetryDataStore("Any connection string");

            Guid correlationId = Guid.NewGuid();
            MySqlConnection connection = new MySqlConnection("Server=AnyServer;Database=AnyDatabase;Port=1234");
            MySqlCommand command = dataStore.CreateTelemetrySearchCommand(connection, new FilterOptions("AnyEventName", correlationId, true));

            Assert.AreEqual(
                RemoveWhitespace(
                    $@"SELECT
                    Timestamp,
                    EventName,
                    CorrelationId,
                    Context
                    FROM TelemetryEvents 
                    WHERE 1 AND CorrelationId = '{correlationId}' AND EventName = 'AnyEventName' AND CorrelationId =
                    (
                        SELECT CorrelationId FROM TelemetryEvents
                        WHERE EventName = 'AnyEventName'
                        ORDER BY Id DESC
                        LIMIT 1
                    ) ORDER BY TIMESTAMP DESC"),
                RemoveWhitespace(command.CommandText));

            Assert.AreEqual(CommandType.Text, command.CommandType);
            Assert.AreEqual(0, command.Parameters.Count);
        }

        private static string RemoveWhitespace(string text)
        {
            return Regex.Replace(text, @"\s+", string.Empty);
        }

        private class TestSqlTelemetryDataStore : SqlTelemetryDataStore<string>
        {
            public TestSqlTelemetryDataStore(string connectionString)
                : base(connectionString)
            {
            }

            public new MySqlCommand CreateTelemetryInsertCommand(MySqlConnection connection, TelemetryEvent<string> telemetry)
            {
                return base.CreateTelemetryInsertCommand(connection, telemetry);
            }

            public new MySqlCommand CreateTelemetrySearchCommand(MySqlConnection connection, FilterOptions filter)
            {
                return base.CreateTelemetrySearchCommand(connection, filter);
            }
        }
    }
}
