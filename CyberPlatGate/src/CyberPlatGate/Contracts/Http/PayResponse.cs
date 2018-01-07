// ReSharper disable InconsistentNaming
namespace CyberPlatGate.Contracts.Http
{
    class PayResponse
    {
		///<summary>
		/// DD.MM.YYYY HH:MM:SS – дата и время запроса на оплату (местное время на сервере Киберплат)
		///</summary>
		public string DATE;
		///<summary>
		/// уникальный идентификатор сессии для данной точки приема, получаемый из запроса, последовательность латинских букв и цифр, длиной не более 20 символов
		///</summary>
		public string SESSION;
		///<summary>
		/// NNN – код ошибки
		///</summary>
		public string ERROR;
		///<summary>
		/// N (0 – успех, 1 – ошибка)
		///</summary>
		public string RESULT;
		///<summary>
		/// уникальный идентификатор платежа в системе КиберПлат, содержит 13 цифр
		///</summary>
		public string TRANSID;
		///<summary>
		/// NNNN – номер входного шлюза, к которому привязана конфигурация провайдера
		///</summary>
		public string GATEWAY_IN;
		///<summary>
		/// NNNN – номер выходного шлюза, через который проводится платеж
		///</summary>
		public string GATEWAY_OUT;
    }
}
