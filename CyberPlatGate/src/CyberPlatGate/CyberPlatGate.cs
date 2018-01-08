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

        public async Task<GateResponse> Check(GateCheckRequest request)
        {
            var clientCheckRequest = new CheckRequest()
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
                PAY_TOOL = ((int) m_Configuration.PAY_TOOL).ToString(),
                TERM_ID = m_Configuration.TERM_ID,
                COMMENT = "",
                ACCEPT_KEYS = null, // will be filled right before sending
                NO_ROUTE = m_Configuration.NO_ROUTE ? "1" : "0",
            };

            var clientCheckResponse = await m_Client.Send(clientCheckRequest).ConfigureAwait(false);

            var gateResponse = new GateResponse() { DisplayInfo = clientCheckResponse.ADDINFO };
            if (clientCheckResponse.RESULT != "0" || clientCheckResponse.ERROR != "0")
            {
                var errCode = int.Parse(clientCheckResponse.ERROR);
                var errDesc = "Unknown error code";
                
                gateResponse.Error = new Error()
                {
                    ErrorCode = errCode,
                    ErrorDescription = errDesc,
                };
                if (ErrorCodes.GateErrorCodes.TryGetValue(errCode, out errDesc))
                    gateResponse.Error.ErrorDescription = errDesc;
            }

            return gateResponse;
        }

        public async Task<GateResponse> CheckAndPay()
        {
            throw new NotImplementedException();
        }

        public async Task<GateResponse> Status()
        {
            throw new NotImplementedException();
        }
        
        public async Task<GateResponse> Limits()
        {
            throw new NotImplementedException();
        }



        // Suitable for CyberPlat API
        private static string printDouble(double d)
        {
            return d.ToString("F2", CultureInfo.InvariantCulture);
        }
    }
}