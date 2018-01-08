using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CyberPlatGate.Components;
using FluentAssertions;
using NUnit.Framework;

namespace CyberPlatGate.Tests.Components
{
    [TestFixture]
    class CyberPlatHttpClientTests
    {
        //TODO[mk] mock manager with NSubstitute and write good unit-tests

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
