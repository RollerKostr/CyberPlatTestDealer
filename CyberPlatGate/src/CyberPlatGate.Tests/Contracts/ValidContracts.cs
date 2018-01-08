using System;
using CyberPlatGate.Components;
using CyberPlatGate.Components.Utility;
using CyberPlatGate.Contracts.Configuration;
using CyberPlatGate.Contracts.Http;

namespace CyberPlatGate.Tests.Contracts
{
    static class ValidContracts
    {
        public static readonly Random Rng = new Random();

        public static CyberPlatHttpClientConfiguration HttpClientConfiguration => new CyberPlatHttpClientConfiguration()
        {
            CheckUrl  = @"http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_check.cgi",
            PayUrl    = @"http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay.cgi",
            StatusUrl = @"http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_status.cgi",
        };

        public static CheckRequest CheckRequest => new CheckRequest()
        {
            SD = "17031",
            AP = "17032",
            OP = "17034",
            DATE = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
            SESSION = RandomStringGenerator.GenerateAlphaNumericalString(20, Rng),
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
    }
}