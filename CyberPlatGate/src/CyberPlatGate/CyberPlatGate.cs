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
    public class CyberPlatGate : ICyberPlatGate
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
                    TransId = null,
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
                TransId = clientPayResponse.TRANSID,
            };
        }

        public async Task<GateStatusResponse> Status(GateStatusRequest gateStatusRequest)
        {
            var clientStatusRequest = new StatusRequest()
            {
                SESSION = gateStatusRequest.Session,
                TRANSID = gateStatusRequest.TransId,
                ACCEPT_KEYS = null, // will be filled right before sending
            };

            var clientStatusResponse = await m_Client.Send(clientStatusRequest).ConfigureAwait(false);

            var response = new GateStatusResponse()
            {
                AuthCode = clientStatusResponse.AUTHCODE,
                Session = clientStatusRequest.SESSION,
                TransId = clientStatusRequest.TRANSID,
                Error = getStatusError(clientStatusResponse),
                Status = new TransferProcessingStatus()
                {
                    IsFinished = false,
                    Code = !string.IsNullOrWhiteSpace(clientStatusResponse.RESULT)
                        ? int.Parse(clientStatusResponse.RESULT)
                        : (int?) null,
                },
            };

            if (response.Status.Code.HasValue)
            {
                string statusDesc;
                if (Constants.GateStatusCodes.TryGetValue(response.Status.Code.Value, out statusDesc))
                    response.Status.Description = statusDesc;
                else if (response.Status.Code > 1 && response.Status.Code < 7)
                    response.Status.Description = "Платеж находится в стадии обработки. Необходимо повторить попытку проверки статуса позднее";

                if (response.Status.Code.Value == 7)
                    response.Status.IsFinished = true;
            }
            else
            {
                if (response.Error.Code == 11)
                    response.Status.Description = "Платеж не зарегистрирован в Киберплат. Необходимо повторить платеж с первого шага с новым номером сессии";
                else
                    response.Status.Description = "Состояние платежа неизвестно. Необходимо повторить попытку проверки статуса позднее";
            }

            return response;
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
                var errDesc = "Произошла неизвестная ошибка";

                error = new Error()
                {
                    Code = errCode,
                    Description = errDesc,
                };
                if (Constants.GateErrorCodes.TryGetValue(errCode, out errDesc))
                    error.Description = errDesc;
            }

            return error;
        }

        private static Error getStatusError(StatusResponse response)
        {
            Error error = null;
            if (response.ERROR != "0")
            {
                var errCode = int.Parse(response.ERROR);
                var errDesc = "Unknown error code";

                error = new Error()
                {
                    Code = errCode,
                    Description = errDesc,
                };
                if (Constants.GateErrorCodes.TryGetValue(errCode, out errDesc))
                    error.Description = errDesc;
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