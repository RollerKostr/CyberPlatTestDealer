using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CyberPlatGate.Contracts.Configuration;
using CyberPlatGate.Contracts.Http;

namespace CyberPlatGate.Components
{
    class CyberPlatHttpClient : ICyberPlatHttpClient
    {
        private Uri CheckUrl { get; }
        private Uri PayUrl { get; }
        private Uri StatusUrl { get; }

        private readonly ICyberPlatHttpClientRequestBuilder m_Builder;
        private readonly HttpClient m_HttpClient;

        public CyberPlatHttpClient(CyberPlatHttpClientConfiguration configuration, ICyberPlatHttpClientRequestBuilder builder, HttpMessageHandler handler = null)
        {
            CheckUrl = ValidateUrl(configuration.CheckUrl);
            PayUrl = ValidateUrl(configuration.PayUrl);
            StatusUrl = ValidateUrl(configuration.StatusUrl);

            if (builder == null) throw new ArgumentNullException(nameof(builder));
            m_Builder = builder;

            m_HttpClient = handler != null ? new HttpClient(handler) : new HttpClient();
            m_HttpClient.Timeout = TimeSpan.FromSeconds(90);
        }

        public async Task<CheckResponse> Send(CheckRequest request)
        {
            var signedData = m_Builder.Build(request);
            var responseTxt = await post(CheckUrl, signedData);
            m_Builder.Verify(responseTxt);
            // TODO[mk] Implement parsing
            return await Task.FromResult(new CheckResponse());
        }

        public async Task<PayResponse> Send(PayRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<StatusResponse> Send(StatusRequest request)
        {
            throw new NotImplementedException();
        }



        private async Task<string> post(Uri url, string data)
        {
            var values = new Dictionary<string, string>
            {
                { "inputmessage", data },
            };

            var response = await m_HttpClient.PostAsync(url, new FormUrlEncodedContent(values));

            return await response.Content.ReadAsStringAsync();
        }

        public static Uri ValidateUrl(string urlStr)
        {
            Uri resultUri;
            if (!Uri.TryCreate(urlStr, UriKind.Absolute, out resultUri)
                || resultUri == null
                || resultUri.Scheme != Uri.UriSchemeHttp && resultUri.Scheme != Uri.UriSchemeHttps)
                throw new ArgumentException($"Invalid URL specified for {nameof(CyberPlatHttpClient)}. Passed value is '{urlStr}'.", nameof(urlStr));

            return resultUri;
        }
    }
}
