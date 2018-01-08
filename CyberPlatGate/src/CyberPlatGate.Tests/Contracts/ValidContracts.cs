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
            Timeout = TimeSpan.FromSeconds(90),
        };

        public static CheckRequest GenerateCheckRequest()
        {
            return new CheckRequest()
            {
                SD = "17031",
                AP = "17032",
                OP = "17034",
                DATE = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                SESSION = RandomStringGenerator.GenerateAlphaNumericString(20, Rng),
                NUMBER = "9261112233",
                ACCOUNT = null,
                AMOUNT = "1234.56",
                AMOUNT_ALL = "1249.99",
                REQ_TYPE = "0",
                PAY_TOOL = "0",
                TERM_ID = null,
                //COMMENT = "TEST Кириллица TEST 0123456789", // CyberPlat not supports cyrillic even if I urlencode in cp1521
                COMMENT = "TEST 0123456789",
                ACCEPT_KEYS = BuilderConfiguration.PublicKeySerial,
                NO_ROUTE = "1",
            };
        }

        public static PayRequest GeneratePayRequest(CheckRequest checkRequest)
        {
            return new PayRequest()
            {
                SD = checkRequest.SD,
                AP = checkRequest.AP,
                OP = checkRequest.OP,
                DATE = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                SESSION = checkRequest.SESSION,
                NUMBER = checkRequest.NUMBER,
                ACCOUNT = checkRequest.ACCOUNT,
                AMOUNT = checkRequest.AMOUNT,
                AMOUNT_ALL = checkRequest.AMOUNT_ALL,
                PAY_TOOL = checkRequest.PAY_TOOL,
                TERM_ID = checkRequest.TERM_ID,
                COMMENT = "TEST 9876543210",
                RRN = RandomStringGenerator.GenerateNumericString(32, Rng),
                ACCEPT_KEYS = BuilderConfiguration.PublicKeySerial,
                NO_ROUTE = "1",
            };
        }

        public static PayResponse GeneratePayResponse()
        {
            return new PayResponse()
            {
                DATE = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                SESSION = RandomStringGenerator.GenerateAlphaNumericString(20, Rng),
                GATEWAY_IN = "9",
                GATEWAY_OUT = "9",
                RESULT = "0",
                ERROR = "0",
                TRANSID = "1000000000000",
            };
        }

        public static StatusRequest GenerateStatusRequest(PayResponse payResponse)
        {
            return new StatusRequest()
            {
                SESSION = payResponse.SESSION,
                TRANSID = payResponse.TRANSID,
                ACCEPT_KEYS = BuilderConfiguration.PublicKeySerial,
            };
        }
    }
}