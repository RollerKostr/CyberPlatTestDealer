namespace CyberPlatGate.Contracts.Gate
{
    /// <summary>Represents generic error with description and optional code</summary>
    public class Error
    {
        public int? ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}
