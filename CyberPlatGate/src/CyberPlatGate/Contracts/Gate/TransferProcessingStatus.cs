namespace CyberPlatGate.Contracts.Gate
{
    /// <summary>Represents status code with description.</summary>
    public class TransferProcessingStatus
    {
        public bool IsFinished { get; set; }
        public int? Code { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Description} (code {Code?.ToString() ?? "null"})";
        }
    }
}
