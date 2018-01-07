using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CyberPlatGate.Components;
using CyberPlatGate.Contracts.Configuration;
using CyberPlatGate.Contracts.Http;
using FluentAssertions;
using NUnit.Framework;

namespace CyberPlatGate.Tests.Components
{
    [TestFixture]
    class CyberPlatHttpClientTests
    {
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
        public async Task CheckTest()
        {
            var builder = new CyberPlatHttpClientRequestBuilder(
                @"C:\Users\RollerKostr\Downloads\29052017_libipriv_win\ActiveX\secret.key",
                @"C:\Users\RollerKostr\Downloads\29052017_libipriv_win\ActiveX\pubkeys.key",
                "1111111111", "17033");
            var client = new CyberPlatHttpClient(ValidConf, builder);

            var response = await client.Send(ValidCheckRequest);
        }

        #region Test cases

        private static CyberPlatHttpClientConfiguration ValidConf => new CyberPlatHttpClientConfiguration()
        {
            CheckUrl = @"http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_check.cgi",
            PayUrl = @"http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay.cgi",
            StatusUrl = @"http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_status.cgi",
        };

        private static CheckRequest ValidCheckRequest => new CheckRequest()
        {
            SD = "17031",
            AP = "17032",
            OP = "17034",
            DATE = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
            SESSION = Guid.NewGuid().ToString("N"), // TODO[mk] Implement random string(20) generator
            NUMBER = "9261112233",
            ACCOUNT = null,
            AMOUNT = "1234.56",
            AMOUNT_ALL = "1249.99",
            REQ_TYPE = "1",
            PAY_TOOL = "0",
            TERM_ID = null,
            COMMENT = "test test 0123456789",
            ACCEPT_KEYS = "64182",
            NO_ROUTE = "1",
        };

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
