using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using CyberPlatGate.Contracts.Http;
using org.CyberPlat;

namespace CyberPlatGate.Components
{
    class CyberPlatHttpClientRequestBuilder : ICyberPlatHttpClientRequestBuilder, IDisposable
    {
        private string SecretKeyPath { get; }
        private string PublicKeyPath { get; }
        private string SecretKeyPassword { get; }
        private string PublicKeySerial { get; }

        public CyberPlatHttpClientRequestBuilder(string secretKeyPath, string publicKeyPath, string secretKeyPassword, string publicKeySerial)
        {
            checkKeyPath(secretKeyPath);
            checkKeyPath(publicKeyPath);

            SecretKeyPath = secretKeyPath;
            PublicKeyPath = publicKeyPath;

            SecretKeyPassword = secretKeyPassword;
            PublicKeySerial = publicKeySerial;

            IPriv.Initialize();
            checkKeys(); // Fail-fast
        }

        public string Build(CheckRequest request)
        {
            return signText(buildCore(request));
        }

        public string Build(PayRequest request)
        {
            return signText(buildCore(request));
        }

        public string Build(StatusRequest request)
        {
            return signText(buildCore(request));
        }

        public void Verify(string response)
        {
            verifyText(response); // should not throw any exception in case of success
        }

        private static string buildCore<T>(T request)
        {
            var dict = toDictionary(request);
            return string.Join("\r\n", dict.Select(kvp => kvp.Key + "=" + kvp.Value));
        }

        private string signText(string inputText)
        {
            IPrivKey secretKey = null;
            try
            {
                secretKey = IPriv.openSecretKey(SecretKeyPath, SecretKeyPassword);
                return secretKey.signText(inputText);
            }
            catch (IPrivException err)
            {
                throw new CryptographicException(err + " (код ошибки = " + err.code + ")", err);
            }
            finally
            {
                secretKey?.closeKey();
            }
        }

        private void verifyText(string inputText)
        {
            IPrivKey publicKey = null;
            try
            {
                publicKey = IPriv.openPublicKey(PublicKeyPath, Convert.ToUInt32(PublicKeySerial, 10));
                publicKey.verifyText(inputText); // If this step is successfull, then signature is valid
            }
            catch (IPrivException err)
            {
                throw new CryptographicException(err + " (код ошибки = " + err.code + ")", err);
            }
            finally
            {
                publicKey?.closeKey();
            }
        }

        private static void checkKeyPath(string keyPath)
        {
            if (string.IsNullOrWhiteSpace(keyPath))
                throw new ArgumentNullException($"Invalid path specified for {nameof(CyberPlatHttpClientRequestBuilder)}. Passed value is '{keyPath}'.", nameof(keyPath));
            if (!File.Exists(keyPath))
                throw new ArgumentException($"There is no file by specified path '{keyPath}'", nameof(keyPath));
        }

        /// <summary>Fail-fast checking of keys opening.</summary>
        private void checkKeys()
        {
            var secretKey = IPriv.openSecretKey(SecretKeyPath, SecretKeyPassword);
            var publicKey = IPriv.openPublicKey(PublicKeyPath, Convert.ToUInt32(PublicKeySerial, 10));
            secretKey?.closeKey();
            publicKey?.closeKey();
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