using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using CyberPlatGate.Contracts.Configurations;
using CyberPlatGate.Contracts.Http;
using org.CyberPlat;

namespace CyberPlatGate.Components
{
    class CyberPlatHttpClientRequestBuilder : ICyberPlatHttpClientRequestBuilder, IDisposable
    {
        private readonly CyberPlatHttpClientRequestBuilderConfiguration m_Configuration;

        public CyberPlatHttpClientRequestBuilder(CyberPlatHttpClientRequestBuilderConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            m_Configuration = configuration;
            
            IPriv.Initialize();

            checkKeys(); // Fail-fast
        }

        public string Build<T>(T request)
        {
            fillAcceptKeysField(request);

            var dict = toDictionary(request);
            var dataStr = string.Join("\r\n", dict.Select(kvp => kvp.Key + "=" + kvp.Value));
            var signedText = signText(dataStr);
            return signedText;
            //return HttpUtility.UrlEncode(signedText, Encoding.GetEncoding(1251)); // Not working anyway (CyberPlat API says it should)
        }

        public void Verify(string response)
        {
            verifyText(response); // should not throw any exception in case of success
        }

        public T Parse<T>(string response) where T : new()
        {
            const string BEGIN_STR = "BEGIN\r\n";
            const string END_STR = "END\r\n";

            var beginIdx = response.IndexOf(BEGIN_STR);
            var endIdx = response.IndexOf(END_STR);
            if (beginIdx == -1)
                throw new HttpParseException($"Can not parse server response. 'BEGIN' word was not found. Response text:{Environment.NewLine}{response}");
            if (endIdx == -1)
                throw new HttpParseException($"Can not parse server response. 'END' word was not found. Response text:{Environment.NewLine}{response}");

            var dataStr = response.Substring(beginIdx + BEGIN_STR.Length, endIdx - beginIdx - BEGIN_STR.Length);
            var lines = dataStr.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            var dict = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                var parts = line.Split('=');
                if (parts.Length != 2)
                    throw new HttpParseException($"Can not parse server response. Line '{line}' does not contain '=' char. Response text:{Environment.NewLine}{response}");
                dict.Add(parts[0], Uri.UnescapeDataString(parts[1]));
            }

            return toObject<T>(dict);
        }

        private string signText(string inputText)
        {
            IPrivKey secretKey = null;
            try
            {
                secretKey = IPriv.openSecretKey(m_Configuration.SecretKeyPath, m_Configuration.SecretKeyPassword);
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
                publicKey = IPriv.openPublicKey(m_Configuration.PublicKeyPath, Convert.ToUInt32(m_Configuration.PublicKeySerial, 10));
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



        private void fillAcceptKeysField(dynamic request)
        {
            if (request is CheckRequest ||
                request is PayRequest ||
                request is StatusRequest)
                request.ACCEPT_KEYS = m_Configuration.PublicKeySerial;
        }

        /// <summary>Fail-fast checking of keys opening.</summary>
        private void checkKeys()
        {
            checkKeyPath(m_Configuration.SecretKeyPath);
            checkKeyPath(m_Configuration.PublicKeyPath);

            IPrivKey secretKey = null;
            IPrivKey publicKey = null;
            try
            {
                secretKey = IPriv.openSecretKey(m_Configuration.SecretKeyPath, m_Configuration.SecretKeyPassword);
                publicKey = IPriv.openPublicKey(m_Configuration.PublicKeyPath, Convert.ToUInt32(m_Configuration.PublicKeySerial, 10));
            }
            finally
            {
                secretKey?.closeKey();
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

        private static Dictionary<string, string> toDictionary<T>(T @object)
        {
            return @object.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => (string) prop.GetValue(@object, null));
        }

        private static T toObject<T>(Dictionary<string, string> dict)
        {
            var obj = Activator.CreateInstance(typeof(T));
            foreach (var kvp in dict)
            {
                var prop = typeof(T).GetProperty(kvp.Key);
                if (prop == null) continue;

                prop.SetValue(obj, kvp.Value, null);
            }

            return (T) obj;
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