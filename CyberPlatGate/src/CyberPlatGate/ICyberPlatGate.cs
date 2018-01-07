namespace CyberPlatGate
{
    /// <summary>
    /// API шлюза КиберПлат<para />
    /// Источник: https://www.cyberplat.ru/download/API_CyberPlat.pdf
    /// </summary>
    public interface ICyberPlatGate
    {
        /// <summary>Получение разрешения на платеж (проверка номера телефона/счета на корректность)</summary>
        void Check();
        /// <summary>Запрос на оплату</summary>
        void Pay();
        /// <summary>Проверка состояния платежа</summary>
        void Status();
        /// <summary>Запрос остатка на счете Контрагента и лимитов</summary>
        void Limits();
    }
}
