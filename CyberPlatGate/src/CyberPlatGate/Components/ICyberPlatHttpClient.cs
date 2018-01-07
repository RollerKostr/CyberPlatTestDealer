using CyberPlatGate.Contracts.Http;

namespace CyberPlatGate.Components
{
    interface ICyberPlatHttpClient
    {
        StatusResponse Send(StatusRequest request);
        PayResponse Send(PayRequest request);
        CheckResponse Send(CheckRequest request);
    }
}