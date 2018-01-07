using System;
using CyberPlatGate.Contracts.Configuration;
using CyberPlatGate.Contracts.Http;

namespace CyberPlatGate.Components
{
    class CyberPlatHttpClient : ICyberPlatHttpClient
    {
        private Uri CheckUrl { get; }
        private Uri PayUrl { get; }
        private Uri StatusUrl { get; }

        private ICyberPlatHttpClientRequestBuilder m_Builder;

        public CyberPlatHttpClient(CyberPlatHttpClientConfiguration configuration, ICyberPlatHttpClientRequestBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            m_Builder = builder;

            CheckUrl  = ValidateUrl(configuration.CheckUrl);
            PayUrl    = ValidateUrl(configuration.PayUrl);
            StatusUrl = ValidateUrl(configuration.StatusUrl);
        }

        public CheckResponse Send(CheckRequest request)
        {
            throw new NotImplementedException();
        }

        public PayResponse Send(PayRequest request)
        {
            throw new NotImplementedException();
        }

        public StatusResponse Send(StatusRequest request)
        {
            throw new NotImplementedException();
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
