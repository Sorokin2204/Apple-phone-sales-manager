using System.Globalization;
using System.Windows.Controls;
using System;

namespace Менеджер
{
    public class NotSelectedComboBoxValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return ((int)value == 0 || (int)value == -1)
                ? new ValidationResult(false, "Обязательное поле для заполнения")
                : ValidationResult.ValidResult;
        }
    }
}