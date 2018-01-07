using CyberPlatGate.Contracts.Http;

namespace CyberPlatGate.Components
{
    interface ICyberPlatHttpClientRequestBuilder
    {
        string Build(StatusRequest request);
        string Build(PayRequest request);
        string Build(CheckRequest request);
    }
}