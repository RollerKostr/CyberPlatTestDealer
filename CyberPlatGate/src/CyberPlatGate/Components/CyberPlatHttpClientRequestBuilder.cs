using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
            return buildCore(request);
        }

        public string Build(PayRequest request)
        {
            return buildCore(request);
        }

        public string Build(StatusRequest request)
        {
            return buildCore(request);
        }

        private static string buildCore<T>(T request)
        {
            var dict = toDictionary(request);
            return string.Join(Environment.NewLine, dict.Select(kvp => kvp.Key + "=" + kvp.Value));
        }



        private static void checkKeyPath(string keyPath)
        {
            if (string.IsNullOrWhiteSpace(keyPath))
                throw new ArgumentNullException($"Invalid path specified for {nameof(CyberPlatHttpClientRequestBuilder)}. Passed value is '{keyPath}'.", nameof(keyPath));
            if (!File.Exists(keyPath))
                throw new ArgumentException($"There is no file by specified path '{keyPath}'", nameof(keyPath));
        }

        private static Dictionary<string, string> toDictionary<T>(T @object)
        {
            return @object.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => (string) prop.GetValue(@object, null));
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