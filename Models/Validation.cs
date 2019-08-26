using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace ActivityCenter.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            var dt = (DateTime) value;
            return dt <= DateTime.Now
                ? ValidationResult.Success
                : new ValidationResult("Birthday cannot be a future date");
        }
    }

    public class Over18Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            var dt = (DateTime) value;
            return DateTime.Now.Year - dt.Year >= 18
                ? ValidationResult.Success
                : new ValidationResult("You must be over 18 to register");
        }
    }

    public class PasswordCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var input = (string) value;

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpLowSpecialChar = new Regex(@"[A-Za-z\d@$!%*?&]+");
            var hasMinimum8Chars = new Regex(@".{8,20}");

            var isValidated = hasNumber.IsMatch(input) && hasUpLowSpecialChar.IsMatch(input) &&
                              hasMinimum8Chars.IsMatch(input);

            return isValidated
                ? ValidationResult.Success
                : new ValidationResult("Password does not meet minimum strength requirements");
        }
    }

    public class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            var dt = (DateTime) value;
            return dt >= DateTime.Now
                ? ValidationResult.Success
                : new ValidationResult("Event date cannot be in the past");
        }
    }
}