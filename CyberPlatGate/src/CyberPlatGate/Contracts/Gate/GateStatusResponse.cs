namespace CyberPlatGate.Contracts.Gate
{
    public class GateStatusResponse
    {
        /// <summary>Determines stage of transfer processing. Not depends on Error field.</summary>
        public TransferProcessingStatus Status { get; set; }
        /// <summary>In case of success this field will be null.</summary>
        public Error Error { get; set; }
        /// <summary>Unique session number used in both Check() and Pay() operations.</summary>
        public string Session { get; set; }
        /// <summary>Unique transfer number in CyberPlat system. Consists of 13 digits.</summary>
        public string TransId { get; set; }
        /// <summary>Authorization code on Receiver side. Consists of digits, max length is 32 characters.</summary>
        public string AuthCode { get; set; }
    }
}
