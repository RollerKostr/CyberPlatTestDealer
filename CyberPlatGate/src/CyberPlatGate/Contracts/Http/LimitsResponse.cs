// ReSharper disable InconsistentNaming
namespace CyberPlatGate.Contracts.Http
{
    class LimitsResponse
    {
		///<summary>
		/// NNN, значение равно 0 при отсутствии ошибки, иначе – код ошибки
		///</summary>
		public string ERROR;
		///<summary>
		/// NNNNNN.NN – остаток доступных Контрагенту средств с учетом лимитов на счете Контрагента и лимитов на точке, (здесь N – цифра, разделитель – точка)
		///</summary>
		public string REST;
		///<summary>
		/// NNNNNN.NN – текущий остаток на счете Контрагента без учета лимитов
		///</summary>
		public string REST_WO_LIMIT;
		///<summary>
		/// NNNNNN.NN – значение установленного на точку лимита, равное максимальному размеру дневного оборота по точке.
		/// Если лимит не установлен, параметр передается без значения
		///</summary>
		public string AP_DAILY_LIMIT;
		///<summary>
		/// NNNNN.NN – лимит на максимальный размер одиночного платежа; если этот лимит не установлен, параметр передается без значения
		///</summary>
		public string AP_PAYMENT_LIMIT;
    }
}
