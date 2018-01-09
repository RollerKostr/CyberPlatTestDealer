// ReSharper disable InconsistentNaming
namespace CyberPlatGate.Contracts.Http
{
    public class StatusResponse
    {
		///<summary>
		/// DD.MM.YYYY HH:MM:SS – дата и время начала транзакции в системе КиберПлат
		///</summary>
		public string DATE { get; set; }
		///<summary>
		/// уникальный идентификатор сессии для данной точки приема, получаемый из запроса, последовательность латинских букв и цифр, длиной не более 20 символов
		///</summary>
		public string SESSION { get; set; }
		///<summary>
		/// NNN – код ошибки, где N – цифра
		///</summary>
		public string ERROR { get; set; }
		///<summary>
		/// N – статус (состояние) платежа:
		/// 1 – был выполнен только запрос на получение разрешения на платёж (запрос на оплату не поступал);
		/// 3 – платеж направлен к Получателю (обрабатывается в системе КиберПлат);
		/// 7 – платеж завершен, в зависимости от значения поля ERROR платеж завершен успешно или неуспешно.
		/// Если ERROR=0, то платеж завершен успешно
		///</summary>
		public string RESULT { get; set; }
		///<summary>
		/// код авторизации платежа на стороне Получателя, цифровое поле, максимальная длина 32 символа
		///</summary>
		public string AUTHCODE { get; set; }
		///<summary>
		/// уникальный идентификатор платежа в системе КиберПлат, содержит 13 цифр
		///</summary>
		public string TRANSID { get; set; }
    }
}
