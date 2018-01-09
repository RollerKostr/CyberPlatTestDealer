using CyberPlatGate.Contracts.Gate;
using DealerSite.Models;

namespace DealerSite.ViewModels
{
    public class PaymentViewModel
    {
        public PaymentInput PaymentInput { get; set; }

        public GateCheckResponse GateCheckResponse { get; set; }
        public GatePayResponse GatePayResponse { get; set; }

        public bool IsCheckSuccessfull => GateCheckResponse != null && GateCheckResponse.Error == null;
        public bool IsPaySuccessfull => GatePayResponse != null && GatePayResponse.Error == null;
    }
}