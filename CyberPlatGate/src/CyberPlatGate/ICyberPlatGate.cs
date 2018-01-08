using System.Threading.Tasks;
using CyberPlatGate.Contracts.Gate;

namespace CyberPlatGate
{
    /// <summary>
    /// API шлюза КиберПлат<para />
    /// Источник: https://www.cyberplat.ru/download/API_CyberPlat.pdf
    /// </summary>
    public interface ICyberPlatGate
    {
        /// <summary>Получение разрешения на платеж (проверка номера телефона/счета на корректность)</summary>
        Task<GateCheckResponse> Check(GateCheckRequest gateCheckRequest);
        /// <summary>Получение разрешения на платеж + выполнение платежа в случае успешного ответа сервера.</summary>
        Task<GatePayResponse> CheckAndPay(GateCheckRequest gateCheckRequest);
        /// <summary>Проверка состояния платежа</summary>
        Task<GateStatusResponse> Status(GateStatusRequest gateStatusRequest);
        /// <summary>Запрос остатка на счете Контрагента и лимитов</summary>
        Task<GateCheckResponse> Limits();
    }
}
