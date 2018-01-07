﻿// ReSharper disable InconsistentNaming
namespace CyberPlatGate.Contracts.Http
{
    class CheckRequest
    {
    	///<summary>
    	/// NNNNN – код Контрагента, где N – цифра
    	///</summary>
    	public string SD { get; set; }
    	///<summary>
    	/// NNNNN – код точки приема, где N – цифра
    	///</summary>
		public string AP { get; set; }
		///<summary>
		/// NNNNN – код оператора точки приема, где N – цифра
    	///</summary>
		public string OP { get; set; }
		///<summary>
		/// DD.MM.YYYY HH:MM:SS – дата и время (UTC) запроса на получение разрешения на платёж (проверку номера телефона/счета) в системе Контрагента
    	///</summary>
		public string DATE { get; set; }
		///<summary>
		/// уникальный идентификатор сессии для данной точки приема, последовательность латинских букв и цифр, длиной не более 20 символов.
		/// Используется для объединения нескольких последовательных запросов различного типа в рамках одной транзакции.
		/// Контрагент самостоятельно генерирует этот идентификатор по своим правилам, соблюдая условие уникальности идентификатора сессии для данной точки приема
    	///</summary>
		public string SESSION { get; set; }
        ///<summary>
        /// параметр платежа, номер телефона или номер счёта плательщика (может быть пустым).
        /// Последовательность латинских букв, цифр и служебных символов, исключая &lt;конец строки&gt;, &lt;перевод строки&gt;, &lt;-&gt;, длина не более 20 символов
        ///</summary>
        public string NUMBER { get; set; }
        ///<summary>
        /// идентификатор плательщика или услуги у Получателя (может быть пустым).
        /// Последовательность цифр, латинских букв, и служебных символов, исключая &lt;конец строки&gt;, &lt;перевод строки&gt;, &lt;-&gt;
        ///</summary>
        public string ACCOUNT { get; set; }
		///<summary>
		/// NNNNN.NN – сумма к зачислению (разделитель – точка), здесь N – цифра
    	///</summary>
		public string AMOUNT { get; set; }
		///<summary>
		/// NNNNN.NN – полная сумма, полученная от плательщика (разделитель – точка), здесь N – цифра
    	///</summary>
		public string AMOUNT_ALL { get; set; }
		///<summary>
		/// 1 – признак того, что производится проверка номера без дальнейшего проведения платежа.
		/// Рекомендуется использовать для фиктивных проверок номера терминалами, когда сумма платежа еще неизвестна
    	///</summary>
		public string REQ_TYPE { get; set; }
		///<summary>
		/// N – тип оплаты:
		/// 0 – наличные средства,
		/// 1 – по банковской карте, эмитированной Банком-парнером («свои» карты),
		/// 2 – по банковской карте, не эмитированной Банком-парнером («чужие» карты).
		/// В случае если Контрагент, не являющийся банком, принимает платеж по банковской карте, значение параметра PAY_TOOL = 2.
		/// При отсутствии параметра значение принимается равным 0
    	///</summary>
		public string PAY_TOOL { get; set; }
		///<summary>
		/// NNNNN – фактический код точки, отправившей платеж. Поле обязательно для заполнения, в случае если фактический код точки, отправившей платеж,
		/// не совпадает со значением AP. Поле используется только для платежей провайдерам МТС и Билайн
    	///</summary>
		public string TERM_ID { get; set; }
		///<summary>
		/// комментарий, назначение платежа: только буквы, цифры и пробелы, длина не более 64 символов
    	///</summary>
		public string COMMENT { get; set; }
        ///<summary>
        /// серийный номер ключа, которым сервер должен подписать ответ, поле может содержать цифры, латинские буквы и знак &lt;-&gt;
        ///</summary>
        public string ACCEPT_KEYS { get; set; }
		///<summary>
		/// N – признак отказа от автоматической переадресации по базе перенесенных номеров:
		/// 1 – отказ, 0 и отсутствие параметра означают выполнение автоматической переадресации на стороне КиберПлат
    	///</summary>
		public string NO_ROUTE { get; set; }
    }
}
