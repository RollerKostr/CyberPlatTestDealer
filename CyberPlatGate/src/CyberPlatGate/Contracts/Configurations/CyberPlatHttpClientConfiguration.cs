namespace CyberPlatGate.Contracts.Configurations
{
    public class CyberPlatHttpClientConfiguration
    {
        public string CheckUrl { get; set; }
        public string PayUrl { get; set; }
        public string StatusUrl { get; set; }

        public int TimeoutSec { get; set; }
    }
}