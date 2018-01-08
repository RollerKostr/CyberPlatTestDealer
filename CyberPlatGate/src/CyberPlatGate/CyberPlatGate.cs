using System;
using System.Threading.Tasks;
using CyberPlatGate.Components;
using CyberPlatGate.Contracts.Configuration;
using CyberPlatGate.Contracts.Gate;
using CyberPlatGate.Contracts.Http;

namespace CyberPlatGate
{
    class CyberPlatGate : ICyberPlatGate
    {
        private readonly ICyberPlatHttpClient m_Client;
        private readonly CyberPlatGateConfiguration m_Configuration;

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
                // TODO[mk] implement filling
            }; 

            var clientCheckResponse = await m_Client.Send(clientCheckRequest).ConfigureAwait(false);
            var gateResponse = new GateResponse() { DisplayInfo = clientCheckResponse.ADDINFO };

            if (clientCheckResponse.RESULT != "0" || clientCheckResponse.ERROR != "0")
            {
                gateResponse.Error = new Error()
                {
                    ErrorCode = int.Parse(clientCheckResponse.ERROR),
                    ErrorDescription = "", // TODO[mk] imlement dictonary
                };
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
    }
}