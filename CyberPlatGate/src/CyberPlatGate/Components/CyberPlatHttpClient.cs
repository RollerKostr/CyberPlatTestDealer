using System;
using CyberPlatGate.Contracts.Configuration;

namespace CyberPlatGate.Components
{
    class CyberPlatHttpClient
    {
        private Uri CheckUrl { get; }
        private Uri PayUrl { get; }
        private Uri StatusUrl { get; }

        public CyberPlatHttpClient(CyberPlatHttpClientConfiguration configuration)
        {
            CheckUrl  = ValidateUrl(configuration.CheckUrl);
            PayUrl    = ValidateUrl(configuration.PayUrl);
            StatusUrl = ValidateUrl(configuration.StatusUrl);
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
