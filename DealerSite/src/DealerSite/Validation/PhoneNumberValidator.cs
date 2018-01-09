using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DealerSite.Validation
{
    public class PhoneNumberValidator : ValidationAttribute
    {
        private readonly Regex m_Rgx = new Regex(@"^\d{10}$", RegexOptions.Compiled);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Please provide Phone number");

            var number = (string) value;

            if (number.StartsWith("+"))
                return new ValidationResult("Enter Phone number without '+' sign");

            if (!m_Rgx.IsMatch(number))
                return new ValidationResult("Phone must consist of 10 digits");

            return ValidationResult.Success;
        }
    }
}