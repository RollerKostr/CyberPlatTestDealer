namespace CyberPlatGate.Contracts.Gate
{
    public class GateStatusRequest
    {
        /// <summary>Unique session number used in both Check() and Pay() operations.</summary>
        public string Session { get; set; }
        /// <summary>Unique transfer number in CyberPlat system. Consists of 13 digits.</summary>
        public string TransId { get; set; }
    }
}
