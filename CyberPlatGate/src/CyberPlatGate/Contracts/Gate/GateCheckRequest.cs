namespace CyberPlatGate.Contracts.Gate
{
    public class GateCheckRequest
    {
        // Only one of this two fields is required
        public string Number { get; set; }
        public string Account { get; set; }

        // If Amount = null, fake check will be performed (like in cellphone payment terminals before actual amount is entered)
        public double? Amount { get; set; }
    }
}
