using System;
using System.IO;
using CyberPlatGate.Contracts.Http;
using org.CyberPlat;

namespace CyberPlatGate.Components
{
    class CyberPlatHttpClientRequestBuilder : ICyberPlatHttpClientRequestBuilder, IDisposable
    {
        private string PublicKeyPath { get; }
        private string SecretKeyPath { get; }

        public CyberPlatHttpClientRequestBuilder(string publicKeyPath, string secretKeyPath)
        {
            checkKeyPath(publicKeyPath);
            checkKeyPath(secretKeyPath);
            PublicKeyPath = publicKeyPath;
            SecretKeyPath = secretKeyPath;

            IPriv.Initialize();
        }

        public string Build(CheckRequest request)
        {
            throw new NotImplementedException();
        }

        public string Build(PayRequest request)
        {
            throw new NotImplementedException();
        }

        public string Build(StatusRequest request)
        {
            throw new NotImplementedException();
        }

        private void checkKeyPath(string keyPath)
        {
            if (string.IsNullOrWhiteSpace(keyPath))
                throw new ArgumentNullException($"Invalid path specified for {nameof(CyberPlatHttpClientRequestBuilder)}. Passed value is '{keyPath}'.", nameof(keyPath));
            if (!File.Exists(keyPath))
                throw new ArgumentException($"There is no file by specified path '{keyPath}'", nameof(keyPath));
        }

        #region IDisposable

        private void releaseUnmanagedResources()
        {
            IPriv.Done();
        }

        private void releaseManagedResources()
        {
            // TODO release managed resources here (call all fields .Dispose() if any)
        }

        protected virtual void Dispose(bool disposing)
        {
            releaseUnmanagedResources();
            if (disposing)
                releaseManagedResources();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CyberPlatHttpClientRequestBuilder()
        {
            Dispose(false);
        }

        #endregion
    }
}