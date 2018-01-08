using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CyberPlatGate.Contracts.Configurations;
using CyberPlatGate.Contracts.Http;

namespace CyberPlatGate.Components
{
    class CyberPlatHttpClient : ICyberPlatHttpClient
    {
        private Uri CheckUrl { get; }
        private Uri PayUrl { get; }
        private Uri StatusUrl { get; }

        private readonly ICyberPlatSignatureManager m_Manager;
        private readonly HttpClient m_HttpClient;

        public CyberPlatHttpClient(ICyberPlatSignatureManager manager, CyberPlatHttpClientConfiguration configuration, HttpMessageHandler handler = null)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            m_Manager = manager;

            CheckUrl = ValidateUrl(configuration.CheckUrl);
            PayUrl = ValidateUrl(configuration.PayUrl);
            StatusUrl = ValidateUrl(configuration.StatusUrl);

            m_HttpClient = handler != null ? new HttpClient(handler) : new HttpClient();
            m_HttpClient.Timeout = TimeSpan.FromSeconds(configuration.TimeoutSec);
        }

        public async Task<CheckResponse> Send(CheckRequest request)
        {
            return await sendCore<CheckRequest, CheckResponse>(CheckUrl, request);
        }

        public async Task<PayResponse> Send(PayRequest request)
        {
            return await sendCore<PayRequest, PayResponse>(PayUrl, request);
        }

        public async Task<StatusResponse> Send(StatusRequest request)
        {
            return await sendCore<StatusRequest, StatusResponse>(StatusUrl, request);
        }



        private async Task<TRes> sendCore<TReq, TRes>(Uri url, TReq request) where TRes : new()
        {
            var signedData = m_Manager.Sign(request);
            var responseTxt = await post(url, signedData).ConfigureAwait(false);
            m_Manager.Verify(responseTxt);

            return m_Manager.Parse<TRes>(responseTxt);
        }

        private async Task<string> post(Uri url, string data)
        {
            var values = new Dictionary<string, string>
            {
                { "inputmessage", data },
            };

            var response = await m_HttpClient.PostAsync(url, new FormUrlEncodedContent(values)).ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        // TODO[mk] make private
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
