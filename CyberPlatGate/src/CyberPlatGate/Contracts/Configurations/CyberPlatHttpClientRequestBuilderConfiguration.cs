namespace CyberPlatGate.Contracts.Configurations
{
    class CyberPlatHttpClientRequestBuilderConfiguration
    {
        public string SecretKeyPath { get; set; }
        public string PublicKeyPath { get; set; }
        public string SecretKeyPassword { get; set; }
        public string PublicKeySerial { get; set; }
    }
}