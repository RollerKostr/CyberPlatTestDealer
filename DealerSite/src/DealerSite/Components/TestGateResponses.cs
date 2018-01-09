using CyberPlatGate.Contracts.Gate;

namespace DealerSite.Components
{
    /// <summary>Useful mock for debugging the site. Remove after testing finished.</summary>
    public static class TestGateResponses
    {
        public static GateCheckResponse ValidCheckResponse => new GateCheckResponse()
        {
            Error = null,
            Session = "abcdefgh",
            DisplayInfo = "Show this text to user",
        };

        public static GateCheckResponse InvalidCheckResponse => new GateCheckResponse()
        {
            Error = new Error()
            {
                Code = 11,
                Description = "Some shit was happened",
            },
            Session = "abcdefgh",
            DisplayInfo = "",
        };

        public static GatePayResponse ValidPayResponse => new GatePayResponse()
        {
            Error = null,
            Session = "abcdefgh",
            RRN = "TransactionRRN",
            TransId = "ServerTransId",
            IsCheckFailed = false,
        };

        public static GatePayResponse InvalidPayResponse => new GatePayResponse()
        {
            Error = new Error()
            {
                Code = 15,
                Description = "Even worse shit was hapened"
            },
            Session = "abcdefgh",
            RRN = "TransactionRRN",
            TransId = "ServerTransId",
            IsCheckFailed = true,
        };
    }
}