using System;
using CyberPlatGate.Contracts.Configurations;

namespace CyberPlatGate.Tests.Contracts
{
    /// <summary>Configurations for integrational tests ONLY!</summary>
    static class TestConfigurations
    {
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

        public static CyberPlatGateConfiguration GateConfiguration => new CyberPlatGateConfiguration()
        {
            SD = "17031",
            AP = "17032",
            OP = "17034",
            PAY_TOOL = PayTool.Cash,
            TERM_ID = null,
            NO_ROUTE = true,
        };
    }
}