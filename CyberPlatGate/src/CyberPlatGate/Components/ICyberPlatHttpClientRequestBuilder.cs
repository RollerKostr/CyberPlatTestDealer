namespace CyberPlatGate.Components
{
    interface ICyberPlatHttpClientRequestBuilder
    {
        string Build<T>(T request);
        void Verify(string response);
        T Parse<T>(string response) where T : new();
    }
}