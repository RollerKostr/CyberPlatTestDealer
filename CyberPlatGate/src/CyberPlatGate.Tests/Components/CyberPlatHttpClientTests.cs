using System;
using System.Collections;
using System.Collections.Generic;
using CyberPlatGate.Components;
using FluentAssertions;
using NUnit.Framework;

namespace CyberPlatGate.Tests.Components
{
    [TestFixture]
    class CyberPlatHttpClientTests
    {
        [Test]
        [TestCaseSource(nameof(validUrls), Category = nameof(validUrls))]
        [TestCaseSource(nameof(invalidUrls), Category = nameof(invalidUrls))]
        public void CyberPlatHttpClientUrlTest(string url)
        {
            Action action = () => { var uri = CyberPlatHttpClient.ValidateUrl(url); };
            var cat = (string) TestContext.CurrentContext.Test.Properties.Get("Category");

            switch (cat)
            {
                case nameof(validUrls):
                    action.ShouldNotThrow<ArgumentException>();
                    break;
                case nameof(invalidUrls):
                    action.ShouldThrow<ArgumentException>();
                    break;
            }
        }

        #region Test cases

        private static IEnumerable<string> validUrls
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

        private static IEnumerable<string> invalidUrls
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
