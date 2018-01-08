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
        Task<GateResponse> Check(GateCheckRequest request);
        /// <summary>Получение разрешения на платеж + выполнение платежа в случае успешного ответа сервера.</summary>
        Task<GateResponse> CheckAndPay();
        /// <summary>Проверка состояния платежа</summary>
        Task<GateResponse> Status();
        /// <summary>Запрос остатка на счете Контрагента и лимитов</summary>
        Task<GateResponse> Limits();
    }
}
