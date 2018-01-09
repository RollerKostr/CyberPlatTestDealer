using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DealerSite.Validation
{
    public class AmountValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Please provide Amount value");

            var amountStr = (string) value;

            double parsedVal;
            if (!double.TryParse(amountStr, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out parsedVal))
                return new ValidationResult("Amount must be integer number or decimal number with dot");

            return ValidationResult.Success;
        }
    }
}