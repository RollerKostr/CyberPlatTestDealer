using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace DealerSite.Components
{
    public class LoggingHttpHandler : DelegatingHandler
    {
        private readonly ILogger m_Logger;

        public LoggingHttpHandler(ILogger logger)
            : this(new HttpClientHandler(), logger)
        {
        }

        public LoggingHttpHandler(HttpMessageHandler innerHandler, ILogger logger)
            : base(innerHandler)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            m_Logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Request:");
            sb.AppendLine(request.ToString());
            if (request.Content != null)
                sb.AppendLine(await request.Content.ReadAsStringAsync());

            m_Logger.Debug(sb.ToString());
            sb.Clear();

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            sb.AppendLine("Response:");
            sb.AppendLine(response.ToString());
            if (response.Content != null)
                sb.AppendLine(await response.Content.ReadAsStringAsync());

            m_Logger.Debug(sb.ToString());

            return response;
        }
    }
}
