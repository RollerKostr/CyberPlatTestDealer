using System.Threading.Tasks;
using CyberPlatGate.Components;
using CyberPlatGate.Contracts.Gate;
using CyberPlatGate.Tests.Contracts;
using FluentAssertions;
using NUnit.Framework;

namespace CyberPlatGate.Tests
{
    [TestFixture]
    //[Ignore("Integrational")]
    class CyberPlatGateTests
    {
        private readonly ICyberPlatGate m_Gate;

        public CyberPlatGateTests()
        {
            var builder = new CyberPlatHttpClientRequestBuilder(TestConfigurations.BuilderConfiguration);
            var client = new CyberPlatHttpClient(builder, TestConfigurations.ClientConfiguration);
            m_Gate = new CyberPlatGate(client, TestConfigurations.GateConfiguration);
        }

        [Test]
        [TestCase(null,         true)]
        [TestCase(0.01,         true)]
        [TestCase(0.0001,       false)]
        [TestCase(1234.5678,    true)]
        [TestCase(9999999.5001, false)]
        public async Task CheckTest(double? amount, bool shouldSucceed)
        {
            var gateCheckRequest = new GateCheckRequest()
            {
                Number = "9261112233",
                Amount = amount,
            };
            var gateResponse = await m_Gate.Check(gateCheckRequest).ConfigureAwait(false);

            if (shouldSucceed)
                gateResponse.Error.Should().BeNull();
            else
                gateResponse.Error.Should().NotBeNull();
        }
    }
}
