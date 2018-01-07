// ReSharper disable InconsistentNaming
namespace CyberPlatGate.Contracts.Http
{
    class LimitsRequest
    {
		///<summary>
		/// NNNNN – код Контрагента, где N – цифра
		///</summary>
		public string SD { get; set; }
		///<summary>
		/// NNNN – код точки приема, где N – цифра
		///</summary>
		public string AP { get; set; }
		///<summary>
		/// NNNNN – код оператора точки приема, где N – цифра
		///</summary>
		public string OP { get; set; }
		///<summary>
		/// уникальный идентификатор сессии для данной точки приема, последовательность латинских букв и цифр, длиной не более 20 символов.
		/// используется для объединения нескольких последовательных запросов различного типа в рамках одной транзакции.
		/// Контрагент самостоятельно генерирует этот идентификатор по своим правилам, соблюдая условие уникальности идентификатора сессии для данной точки приема.
		/// Необязательный параметр
		///</summary>
		public string SESSION { get; set; }
		///<summary>
		/// серийный номер ключа, которым сервер должен подписать ответ. Поле может содержать цифры, латинские буквы и знак &lt;-&gt;
		///</summary>
		public string ACCEPT_KEYS { get; set; }
    }
}
