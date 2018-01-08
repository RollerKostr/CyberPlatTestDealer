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

        public static CyberPlatHttpClientRequestBuilderConfiguration BuilderConfiguration => new CyberPlatHttpClientRequestBuilderConfiguration()
        {
            SecretKeyPath = @"C:\Users\RollerKostr\Downloads\29052017_libipriv_win\ActiveX\secret.key",
            PublicKeyPath = @"C:\Users\RollerKostr\Downloads\29052017_libipriv_win\ActiveX\pubkeys.key",
            SecretKeyPassword = @"1111111111",
            PublicKeySerial = @"64182",
        };

        public static CyberPlatHttpClientConfiguration ClientConfiguration => new CyberPlatHttpClientConfiguration()
        {
            CheckUrl  = @"https://ru-demo.cyberplat.com/cgi-bin/es/es_pay_check.cgi",
            PayUrl    = @"https://ru-demo.cyberplat.com/cgi-bin/es/es_pay.cgi",
            StatusUrl = @"https://ru-demo.cyberplat.com/cgi-bin/es/es_pay_status.cgi",
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
            //COMMENT = "TEST Кириллица TEST 0123456789", // CyberPlat not supports cyrillic even if I urlencode in cp1521
            COMMENT = "TEST 0123456789",
            ACCEPT_KEYS = "64182",
            NO_ROUTE = "1",
        };
    }
}