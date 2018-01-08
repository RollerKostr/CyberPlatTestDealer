using System;
using System.Globalization;
using System.Threading.Tasks;
using CyberPlatGate.Components;
using CyberPlatGate.Components.Utility;
using CyberPlatGate.Contracts.Configurations;
using CyberPlatGate.Contracts.Gate;
using CyberPlatGate.Contracts.Http;

namespace CyberPlatGate
{
    class CyberPlatGate : ICyberPlatGate
    {
        private readonly ICyberPlatHttpClient m_Client;
        private readonly CyberPlatGateConfiguration m_Configuration;

        private readonly Random m_Rng = new Random();
        private const double FAKE_AMOUNT = 100.50d;

        public CyberPlatGate(ICyberPlatHttpClient client, CyberPlatGateConfiguration configuration)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            m_Client = client;
            m_Configuration = configuration;
        }

        public async Task<GateCheckResponse> Check(GateCheckRequest gateCheckRequest)
        {
            var clientCheckRequest = prepareCheckRequest(gateCheckRequest);

            var clientCheckResponse = await m_Client.Send(clientCheckRequest).ConfigureAwait(false);

            return new GateCheckResponse()
            {
                DisplayInfo = clientCheckResponse.ADDINFO,
                Error = getCheckPayError(clientCheckResponse),
                Session = clientCheckRequest.SESSION,
            };
        }

        public async Task<GatePayResponse> CheckAndPay(GateCheckRequest gateCheckRequest)
        {
            var clientCheckRequest = prepareCheckRequest(gateCheckRequest);

            var clientCheckResponse = await m_Client.Send(clientCheckRequest).ConfigureAwait(false);

            var error = getCheckPayError(clientCheckResponse);
            if (error != null)
                return new GatePayResponse()
                {
                    Error = error,
                    IsCheckFailed = true,
                    Session = clientCheckRequest.SESSION,
                    RRN = null,
                };

            var clientPayRequest = new PayRequest()
            {
                SD = clientCheckRequest.SD,
                AP = clientCheckRequest.AP,
                OP = clientCheckRequest.OP,
                DATE = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                SESSION = clientCheckRequest.SESSION,
                NUMBER = clientCheckRequest.NUMBER,
                ACCOUNT = clientCheckRequest.ACCOUNT,
                AMOUNT = clientCheckRequest.AMOUNT,
                AMOUNT_ALL = clientCheckRequest.AMOUNT_ALL,
                PAY_TOOL = clientCheckRequest.PAY_TOOL,
                TERM_ID = clientCheckRequest.TERM_ID,
                COMMENT = "",
                RRN = RandomStringGenerator.GenerateNumericString(32, m_Rng),
                ACCEPT_KEYS = null, // will be filled right before sending
                NO_ROUTE = clientCheckRequest.NO_ROUTE,
            };

            var clientPayResponse = await m_Client.Send(clientPayRequest).ConfigureAwait(false);

            return new GatePayResponse()
            {
                Error = getCheckPayError(clientPayResponse),
                IsCheckFailed = false,
                Session = clientCheckRequest.SESSION,
                RRN = clientPayRequest.RRN,
            };
        }

        public async Task<GateCheckResponse> Status()
        {
            throw new NotImplementedException();
        }
        
        public async Task<GateCheckResponse> Limits()
        {
            throw new NotImplementedException();
        }



        private CheckRequest prepareCheckRequest(GateCheckRequest request)
        {
            return new CheckRequest()
            {
                SD = m_Configuration.SD,
                AP = m_Configuration.AP,
                OP = m_Configuration.OP,
                DATE = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                SESSION = RandomStringGenerator.GenerateAlphaNumericString(20, m_Rng),
                NUMBER = request.Number,
                ACCOUNT = request.Number != null ? string.Empty : request.Account,
                AMOUNT = request.Amount.HasValue ? printDouble(request.Amount.Value) : printDouble(FAKE_AMOUNT),
                AMOUNT_ALL = request.Amount.HasValue ? printDouble(request.Amount.Value) : printDouble(FAKE_AMOUNT),
                REQ_TYPE = request.Amount == null ? "1" : "0",
                PAY_TOOL = ((int)m_Configuration.PAY_TOOL).ToString(),
                TERM_ID = m_Configuration.TERM_ID,
                COMMENT = "",
                ACCEPT_KEYS = null, // will be filled right before sending
                NO_ROUTE = m_Configuration.NO_ROUTE ? "1" : "0",
            };
        }

        private static Error getCheckPayError(dynamic response)
        {
            if (!(response is CheckResponse || response is PayResponse))
                return null;

            Error error = null;
            if (response.RESULT != "0" || response.ERROR != "0")
            {
                var errCode = int.Parse(response.ERROR);
                var errDesc = "Unknown error code";

                error = new Error()
                {
                    ErrorCode = errCode,
                    ErrorDescription = errDesc,
                };
                if (ErrorCodes.GateErrorCodes.TryGetValue(errCode, out errDesc))
                    error.ErrorDescription = errDesc;
            }

            return error;
        }

        // Suitable for CyberPlat API
        private static string printDouble(double d)
        {
            return d.ToString("F2", CultureInfo.InvariantCulture);
        }
    }
}