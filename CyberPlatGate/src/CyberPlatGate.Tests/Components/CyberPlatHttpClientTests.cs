using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CyberPlatGate.Components;
using CyberPlatGate.Components.Utility;
using CyberPlatGate.Tests.Contracts;
using FluentAssertions;
using NUnit.Framework;

namespace CyberPlatGate.Tests.Components
{
    [TestFixture]
    class CyberPlatHttpClientTests
    {
        //TODO[mk] mock builder with NSubstitute and write good unit-tests

        [Test]
        [TestCaseSource(nameof(ValidUrls), Category = nameof(ValidUrls))]
        [TestCaseSource(nameof(InvalidUrls), Category = nameof(InvalidUrls))]
        // TODO[mk] change to InitTest() and make ValidateUrl() private
        public void UrlTest(string url)
        {
            Action action = () => { var uri = CyberPlatHttpClient.ValidateUrl(url); };
            var cat = (string) TestContext.CurrentContext.Test.Properties.Get("Category");

            switch (cat)
            {
                case nameof(ValidUrls):
                    action.ShouldNotThrow<ArgumentException>();
                    break;
                case nameof(InvalidUrls):
                    action.ShouldThrow<ArgumentException>();
                    break;
            }
        }

        [Test]
        //[Ignore("Integrational")]
        public async Task CheckAndPayTest()
        {
            using (var builder = new CyberPlatHttpClientRequestBuilder(ValidContracts.BuilderConfiguration))
            {
                var handler = new ConsoleLoggingHttpHandler();
                var client = new CyberPlatHttpClient(ValidContracts.ClientConfiguration, builder, handler);

                var checkRequest = ValidContracts.GenerateCheckRequest();
                var checkResponse = await client.Send(checkRequest).ConfigureAwait(false);
                if (checkResponse.RESULT != "0" && checkResponse.ERROR != "0")
                    throw new Exception("Server returns response with error.");

                var payResponse = await client.Send(ValidContracts.GeneratePayRequest(checkRequest)).ConfigureAwait(false);
            }
        }

        #region Test cases

        private static IEnumerable<string> ValidUrls
        {
            get
            {
                yield return @"http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_check.cgi";
				yield return @"http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay.cgi";
				yield return @"http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_status.cgi";
				yield return @"https://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_check.cgi";
				yield return @"https://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay.cgi";
				yield return @"https://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_status.cgi";
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
