namespace CyberPlatGate.Contracts.Gate
{
    public class GatePayResponse
    {
        /// <summary>In case of success this field will be null. It also means you can continue to Pay() operation.</summary>
        public Error Error { get; set; }
        /// <summary>Determines what operation stage causes an error (if it was at all).</summary>
        public bool IsCheckFailed { get; set; }
        /// <summary>Unique session number used in Pay() operation.</summary>
        public string Session { get; set; }
        /// <summary>Unique transfer number used in Pay() operation.</summary>
        public string RRN { get; set; }
    }
}
