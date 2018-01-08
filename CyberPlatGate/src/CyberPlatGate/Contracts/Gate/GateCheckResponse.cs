namespace CyberPlatGate.Contracts.Gate
{
    public class GateCheckResponse
    {
        /// <summary>In case of success this field will be null. It also means you can continue to Pay() operation.</summary>
        public Error Error { get; set; }
        /// <summary>If not null you should display this info to user.</summary>
        public string DisplayInfo { get; set; }
        /// <summary>Unique session number used in Pay() operation.</summary>
        public string Session { get; set; }
    }
}
