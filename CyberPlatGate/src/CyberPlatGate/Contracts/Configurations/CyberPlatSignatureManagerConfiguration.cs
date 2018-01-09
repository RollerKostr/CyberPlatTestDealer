namespace CyberPlatGate.Contracts.Configurations
{
    public class CyberPlatSignatureManagerConfiguration
    {
        public string SecretKeyPath { get; set; }
        public string PublicKeyPath { get; set; }
        public string SecretKeyPassword { get; set; }
        public string PublicKeySerial { get; set; }
    }
}