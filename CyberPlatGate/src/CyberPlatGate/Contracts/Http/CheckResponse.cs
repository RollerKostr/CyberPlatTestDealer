// ReSharper disable InconsistentNaming
namespace CyberPlatGate.Contracts.Http
{
    public class CheckResponse
    {
		///<summary>
		/// DD.MM.YYYY HH:MM:SS – дата и время получения запроса на получение разрешения на платёж (проверку номера телефона/счета), местное время на сервере Киберплат
		///</summary>
		public string DATE { get; set; }
		///<summary>
		/// уникальный идентификатор сессии для данной точки приема, получаемый из запроса, последовательность латинских букв и цифр длиной не более 20 символов
		///</summary>
		public string SESSION { get; set; }
		///<summary>
		/// NNNN – код ошибки, где N – цифра
		///</summary>
		public string ERROR { get; set; }
		///<summary>
		/// N – признак успеха запроса: 0 – успех, 1 – ошибка
		///</summary>
		public string RESULT { get; set; }
		///<summary>
		/// символьное поле, название Получателя (поставщика услуг), в пользу которого выполняется платеж (не более 20 символов)
		///</summary>
		public string OPNAME { get; set; }
		///<summary>
		/// идентификатор плательщика или идентификатор услуги у Получателя
		///</summary>
		public string ACCOUNT { get; set; }
		///<summary>
		/// текст расшифровки ошибки, символьное поле, значение пусто в случае удачного платежа
		///</summary>
		public string ERRMSG { get; set; }
		///<summary>
		/// код авторизации на стороне Получателя, цифровое поле, максимальная длина – 32 символа
		///</summary>
		public string AUTHCODE { get; set; }
		///<summary>
		/// уникальный идентификатор платежа в системе КиберПлат, содержит 13 цифр
		///</summary>
		public string TRANSID { get; set; }
		///<summary>
		/// ХХ…Х – информационное сообщение для плательщика, текст в кодировке Windows 1251.
		/// Сообщение, передаваемое в поле ADDINFO, должно выводиться на экран клиентского приложения до отправки запроса на оплату
		///</summary>
		public string ADDINFO { get; set; }
		///<summary>
		/// текущий баланс счета плательщика у выбранного поставщика услуг
		///</summary>
		public string BALANCE { get; set; }
		///<summary>
		/// сумма к оплате, рекомендованная поставщиком услуг
		///</summary>
		public string PRICE_RECOMMEND { get; set; }
		///<summary>
		/// NNNNNNN.NN – остаток средств на счёте Контрагента
		///</summary>
		public string REST { get; set; }
		///<summary>
		/// NNNNNNN.NN – доступная сумма пополнения, остаток лимита пополнения счета в течение текущего календарного месяца.
		/// Необязательный параметр, возвращается в некоторых случаях, когда имеется лимит пополнения на одни реквизиты в течение месяца
		///</summary>
		public string RESTLIMIT { get; set; }
		///<summary>
		/// NNNN.NN – сумма, ожидаемая поставщиком услуг. Необязательный параметр, возвращается в случае фиксированной суммы платежа за услугу
		///</summary>
		public string PRICE { get; set; }
		///<summary>
		/// NNNN – номер входного шлюза, к которому привязана конфигурация провайдера
		///</summary>
		public string GATEWAY_IN { get; set; }
		///<summary>
		/// NNNN – номер выходного шлюза, через который проводится платеж
		///</summary>
		public string GATEWAY_OUT { get; set; }
    }
}
