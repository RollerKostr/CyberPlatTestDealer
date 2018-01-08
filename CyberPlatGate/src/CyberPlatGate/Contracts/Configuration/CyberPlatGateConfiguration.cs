﻿namespace CyberPlatGate.Contracts.Configuration
{
    public enum PayTool
    {
        Cash = 0,
        CardPartner = 1,
        CardOther = 2,
    }

    public class CyberPlatGateConfiguration
    {
        ///<summary>Код Контрагента</summary>
        public string SD { get; set; }
        ///<summary>Код точки приема</summary>
        public string AP { get; set; }
        ///<summary>Код оператора точки приема</summary>
        public string OP { get; set; }
        ///<summary>
        /// Тип оплаты:
        /// 0 – наличные деньги,
        /// 1 – по банковской карте, эмитированной Банком-партнером («свои» карты),
        /// 2 – по банковской карте, не эмитированной Банком-партнером («чужие» карты).
        /// В случае если Контрагент, не являющийся банком, принимает платеж по банковской карте, значение параметра равно 2.
        /// При отсутствии параметра значение принимается равным нулю.
        ///</summary>
        public PayTool PAY_TOOL { get; set; }
        ///<summary>
        /// Фактический код точки, отправившей платеж.
        /// Поле обязательно для заполнения, в случае если фактический код точки, отправившей платеж, не совпадает со значением AP (код точки приема).
        /// Используется только для платежей провайдерам МТС и Билайн
        ///</summary>
        public string TERM_ID { get; set; }
        ///<summary>
        /// Признак отказа от автоматической переадресации по базе перенесенных номеров:
        /// Значение False и отсутствие параметра означают выполнение автоматической переадресации на стороне КиберПлат.
        ///</summary>
        public bool NO_ROUTE { get; set; }
    }
}