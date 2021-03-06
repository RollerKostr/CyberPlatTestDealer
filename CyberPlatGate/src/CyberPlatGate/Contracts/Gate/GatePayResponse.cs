﻿namespace CyberPlatGate.Contracts.Gate
{
    public class GatePayResponse
    {
        /// <summary>In case of success this field will be null.</summary>
        public Error Error { get; set; }
        /// <summary>Determines what operation stage causes an error (if it was at all).</summary>
        public bool IsCheckFailed { get; set; }
        /// <summary>Unique session number used in both Check() and Pay() operations.</summary>
        public string Session { get; set; }
        /// <summary>Unique transfer number used in Pay() operation.</summary>
        public string RRN { get; set; }
        /// <summary>Unique transfer number in CyberPlat system. Consists of 13 digits.</summary>
        public string TransId { get; set; }
    }
}
