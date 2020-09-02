using System.Linq;
using NUnit.Framework;
using TransmissionManager.Config;

namespace PsTransmissionManager.Tests
{
    public class TransmissionConfigTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test()
        {
            var setCmdlet = new SetTransmissionConfigCmdlet
            {
                Host = "http://192.168.1.20:49091/transmission/rpc",
                User = "qnap",
                Password = "qnap"
            };

            var setResults = setCmdlet.Invoke().OfType<object>().ToList();

            var getCmdlet = new GetTransmissionConfigCmdlet();

            var getResults = setCmdlet.Invoke().OfType<object>().ToList();

            Assert.Pass();
        }
    }
}