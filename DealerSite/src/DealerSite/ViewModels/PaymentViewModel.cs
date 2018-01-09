using CyberPlatGate.Contracts.Gate;

namespace DealerSite.ViewModels
{
    public class PaymentViewModel
    {
        public GateCheckResponse GateCheckResponse { get; set; }
        public GatePayResponse GatePayResponse { get; set; }

        public bool IsCheckSuccessfull => GateCheckResponse?.Error == null;
        public bool IsPaySuccessfull => GatePayResponse?.Error == null;
    }
}