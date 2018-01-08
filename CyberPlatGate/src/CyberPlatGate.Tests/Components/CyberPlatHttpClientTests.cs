﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CyberPlatGate.Components;
using CyberPlatGate.Components.Utility;
using CyberPlatGate.Contracts.Configuration;
using CyberPlatGate.Contracts.Http;
using CyberPlatGate.Tests.Contracts;
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
            using (var builder = new CyberPlatHttpClientRequestBuilder(
                @"C:\Users\RollerKostr\Downloads\29052017_libipriv_win\ActiveX\secret.key",
                @"C:\Users\RollerKostr\Downloads\29052017_libipriv_win\ActiveX\pubkeys.key",
                "1111111111", "17033"))
            {
                var handler = new ConsoleLoggingHttpHandler();
                var client = new CyberPlatHttpClient(ValidContracts.HttpClientConfiguration, builder, handler);

                var response = await client.Send(ValidContracts.CheckRequest);
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
