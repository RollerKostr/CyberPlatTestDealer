using System;

namespace CyberPlatGate.Contracts.Configurations
{
    class CyberPlatHttpClientConfiguration
    {
        public string CheckUrl { get; set; }
        public string PayUrl { get; set; }
        public string StatusUrl { get; set; }

        public TimeSpan Timeout { get; set; }
    }
}