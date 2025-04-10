using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace gamelauncher.Validation
{
    public class PasswordValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,20}$";
            if(!Regex.IsMatch(value.ToString(), passwordPattern))
            {
                return new ValidationResult(false, "Плохой пароль");
            }
            return ValidationResult.ValidResult;
        }
    }

    public class RepeatPasswordValidation
    {

    }
}
