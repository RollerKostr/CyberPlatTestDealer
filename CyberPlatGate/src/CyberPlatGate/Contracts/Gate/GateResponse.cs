namespace CyberPlatGate.Contracts.Gate
{
    /// <summary>Represents generic gate response.</summary>
    public class GateResponse
    {
        /// <summary>In case of success this field will be null. It also means you can continue to Pay() operation.</summary>
        public Error Error { get; set; }
        /// <summary>If not null you should display this info to user.</summary>
        public string DisplayInfo { get; set; }
    }
}
