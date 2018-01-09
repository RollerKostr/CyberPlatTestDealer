namespace CyberPlatGate.Components
{
    public interface ICyberPlatSignatureManager
    {
        string Sign<T>(T request);
        void Verify(string response);
        T Parse<T>(string response) where T : new();
    }
}