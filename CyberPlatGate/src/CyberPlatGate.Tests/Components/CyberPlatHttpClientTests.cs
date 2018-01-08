using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CyberPlatGate.Components;
using CyberPlatGate.Contracts.Configurations;
using CyberPlatGate.Contracts.Http;
using CyberPlatGate.Tests.Contracts;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NUnit.Framework;

namespace CyberPlatGate.Tests.Components
{
    [TestFixture]
    class CyberPlatHttpClientTests
    {
        private readonly ICyberPlatSignatureManager m_ManagerMock = Substitute.For<ICyberPlatSignatureManager>();
        private readonly ICyberPlatHttpClient m_Client;

        public CyberPlatHttpClientTests()
        {
            var dummyConf = new CyberPlatHttpClientConfiguration()
            {
                CheckUrl  = @"http://google.com",
                PayUrl    = @"http://google.com",
                StatusUrl = @"http://google.com",
                TimeoutSec = 10,
            };
            m_Client = new CyberPlatHttpClient(m_ManagerMock, dummyConf);
        }

        [Test]
        [TestCaseSource(typeof(CyberPlatHttpClientTests), nameof(ConfigurationTestCases))]
        public void InitializingTest(CyberPlatHttpClientConfiguration conf, bool shouldSucceed)
        {
            Action action = () =>
            {
                var client = new CyberPlatHttpClient(m_ManagerMock, conf);
            };

            if (shouldSucceed)
                action.ShouldNotThrow<ArgumentException>();
            else
                action.ShouldThrow<ArgumentException>();
        }

        [Test]
        [Ignore("Can not mock HttpClient, will perform real calls to URL")]
        public async Task SendTest()
        {
            var request = new CheckRequest();

            m_ManagerMock.ClearSubstitute();
            m_ManagerMock.Sign(request).Returns(request.GetType().Name);
            m_ManagerMock.Parse<CheckRequest>(null).ReturnsForAnyArgs((CheckRequest)null);

            var response = await m_Client.Send(request).ConfigureAwait(false);

            Received.InOrder(() =>
            {
                m_ManagerMock.Sign(request);
                m_ManagerMock.Verify(Arg.Any<string>());
                m_ManagerMock.Parse<CheckResponse>(Arg.Any<string>());
            });
        }

        #region Test cases

        public static IEnumerable ConfigurationTestCases
        {
            get
            {
                return new[] {new TestCaseData(TestConfigurations.ClientConfiguration, true)}.Concat(
                    InvalidUrls.Select(url => new TestCaseData(new CyberPlatHttpClientConfiguration() {CheckUrl = url}, false)));
            }
        }

        private static IEnumerable<string> InvalidUrls
        {
            get
            {
                yield return @"http:ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_check.cgi";
                yield return @"/cgi-bin/DealerSertification/de_pay.cgi";
                yield return @"htttttp://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_status.cgi";
                yield return @"http://";
                yield return @"abcdef";
            }
        }

        #endregion Test cases
    }
}
