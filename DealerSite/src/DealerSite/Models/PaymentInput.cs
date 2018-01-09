using DealerSite.Validation;

namespace DealerSite.Models
{
    public class PaymentInput
    {
        [PhoneNumberValidator]
        public string Number { get; set; }
        [AmountValidator]
        public string Amount { get; set; }
    }
}