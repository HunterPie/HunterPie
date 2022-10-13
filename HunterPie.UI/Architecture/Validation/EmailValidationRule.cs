using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace HunterPie.UI.Architecture.Validation;
public class EmailValidationRule : ValidationRule
{
    private const string EMAIL_PATTERN = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

    private const string INVALID_EMAIL = "VALIDATION_FOR_EMAIL_FAIL";

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is not string email)
            throw new ArgumentException("value must be string");

        if (string.IsNullOrEmpty(email))
            return new ValidationResult(true, INVALID_EMAIL);

        bool isValid = Regex.IsMatch(email, EMAIL_PATTERN);

        return new ValidationResult(isValid, isValid ? null : INVALID_EMAIL);
    }
}
