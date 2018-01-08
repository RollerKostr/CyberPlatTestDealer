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
        [TestCase(null,         true,  "Fake Check() will be performed")]
        [TestCase(0.01,         true,  "0.01 RUB is valid amount for checking")]
        [TestCase(0.0001,       false, "0.0001 will be serialized into 0.00 and will be failed during Check()")]
        [TestCase(1234.5678,    true,  "1234.5678 will be serialized into 1234.57 and it is valid amount for checking")]
        [TestCase(9999999.5001, false, "Too big amount will be rejected to check")]
        public async Task CheckTest(double? amount, bool shouldSucceed, string reason)
        {
            var gateCheckRequest = new GateCheckRequest()
            {
                Number = "9261112233",
                Amount = amount,
            };
            var gateResponse = await m_Gate.Check(gateCheckRequest).ConfigureAwait(false);

            if (shouldSucceed)
                gateResponse.Error.Should().BeNull(reason);
            else
                gateResponse.Error.Should().NotBeNull(reason);
        }

        [Test]
        [TestCase(null,         false, "Pay() can not be performed after fake Check()")]
        [TestCase(0.01,         true,  "0.01 RUB is valid amount for payment")]
        [TestCase(0.0001,       false, "0.0001 will be serialized into 0.00 and will be failed during Check()")]
        [TestCase(1234.5678,    true,  "1234.5678 will be serialized into 1234.57 and it is valid amount for payment")]
        [TestCase(9999999.5001, false, "Too big amount will be rejected to pay")]
        public async Task CheckPayTest(double? amount, bool shouldSucceed, string reason)
        {
            var gateCheckRequest = new GateCheckRequest()
            {
                Number = "9261112233",
                Amount = amount,
            };
            var gateResponse = await m_Gate.CheckAndPay(gateCheckRequest).ConfigureAwait(false);

            if (shouldSucceed)
                gateResponse.Error.Should().BeNull(reason);
            else
                gateResponse.Error.Should().NotBeNull(reason);
        }
    }
}
