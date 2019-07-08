using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace Eventcore.Telemetry.Data.UnitTests
{
    [TestClass]
    public class TelemetryEventTests
    {
        [TestMethod]
        public void TelemetryEventObjectSetsPropertiesToExpectedValues()
        {
            string eventName = "AnyEvent";
            Guid correlationId = Guid.NewGuid();
            DateTime timestamp = DateTime.Now;
            string context = "Any context";

            TelemetryEvent<string> telemetry = new TelemetryEvent<string>(
                eventName,
                correlationId,
                timestamp,
                context);

            Assert.AreEqual(eventName, telemetry.EventName);
            Assert.AreEqual(correlationId, telemetry.CorrelationId);
            Assert.AreEqual(timestamp, telemetry.Timestamp);
            Assert.AreEqual(context, telemetry.Context);
        }

        [TestMethod]
        public void TelemetryEventObjectIsSerializable()
        {
            TelemetryEvent<string> telemetry = new TelemetryEvent<string>(
                "AnyEvent",
                Guid.NewGuid(),
                DateTime.Now,
                "AnyContext");

            string serializedTelemetry = JsonConvert.SerializeObject(telemetry);
            TelemetryEvent<string> deserializedTelemetry = JsonConvert.DeserializeObject<TelemetryEvent<string>>(serializedTelemetry);

            Assert.AreEqual(telemetry.EventName, deserializedTelemetry.EventName);
            Assert.AreEqual(telemetry.CorrelationId, deserializedTelemetry.CorrelationId);
            Assert.AreEqual(telemetry.Timestamp, deserializedTelemetry.Timestamp);
            Assert.AreEqual(telemetry.Context, deserializedTelemetry.Context);
        }
    }
}
