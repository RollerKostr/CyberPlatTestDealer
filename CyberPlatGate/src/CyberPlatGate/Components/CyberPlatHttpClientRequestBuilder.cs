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
    class CyberPlatHttpClientRequestBuilderConfiguration
    {
        public string SecretKeyPath { get; set; }
        public string PublicKeyPath { get; set; }
        public string SecretKeyPassword { get; set; }
        public string PublicKeySerial { get; set; }
    }

    class CyberPlatHttpClientRequestBuilder : ICyberPlatHttpClientRequestBuilder, IDisposable
    {
        private readonly CyberPlatHttpClientRequestBuilderConfiguration m_Conf;

        // TODO[mk] encapsulate all parameters into separate class
        public CyberPlatHttpClientRequestBuilder(CyberPlatHttpClientRequestBuilderConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            m_Conf = configuration;
            
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
                secretKey = IPriv.openSecretKey(m_Conf.SecretKeyPath, m_Conf.SecretKeyPassword);
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
                publicKey = IPriv.openPublicKey(m_Conf.PublicKeyPath, Convert.ToUInt32(m_Conf.PublicKeySerial, 10));
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

        

        /// <summary>Fail-fast checking of keys opening.</summary>
        private void checkKeys()
        {
            checkKeyPath(m_Conf.SecretKeyPath);
            checkKeyPath(m_Conf.PublicKeyPath);

            var secretKey = IPriv.openSecretKey(m_Conf.SecretKeyPath, m_Conf.SecretKeyPassword);
            var publicKey = IPriv.openPublicKey(m_Conf.PublicKeyPath, Convert.ToUInt32(m_Conf.PublicKeySerial, 10));
            secretKey?.closeKey();
            publicKey?.closeKey();
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