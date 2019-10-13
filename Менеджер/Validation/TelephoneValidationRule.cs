using System.Globalization;
using System.Windows.Controls;
using System;

namespace Менеджер
{
    public class TelephoneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Ошибка")
                : ValidationResult.ValidResult;
        }
    }
}