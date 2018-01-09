using CyberPlatGate.Contracts.Gate;
using DealerSite.Models;

namespace DealerSite.ViewModels
{
    public class PaymentViewModel
    {
        public PaymentInput PaymentInput { get; set; }
        public StatusInput StatusInput { get; set; }

        public GateCheckResponse GateCheckResponse { get; set; }
        public GatePayResponse GatePayResponse { get; set; }
        public GateStatusResponse GateStatusResponse { get; set; }

        public bool IsCheckSuccessfull => GateCheckResponse != null && GateCheckResponse.Error == null;
        public bool IsPaySuccessfull => GatePayResponse != null && GatePayResponse.Error == null;
        public bool IsStatusSuccessfull => GateStatusResponse != null && GateStatusResponse.Error == null && GateStatusResponse.Status.IsFinished;

        public string ExceptionMessage { get; set; }
    }
}