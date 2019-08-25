using System.ComponentModel.DataAnnotations;
using System;
using System.Text.RegularExpressions;

namespace ActivityCenter.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public FutureDateAttribute()
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var dt = (DateTime)value;
            if (dt <= DateTime.Now)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Birthday cannot be a future date");
        }
    }
    public class Over18Attribute : ValidationAttribute
    {
        public Over18Attribute()
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var dt = (DateTime)value;
            if (DateTime.Now.Year - dt.Year >= 18)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("You must be over 18 to register");
        }
        
        
    }

    public class PasswordCheckAttribute : ValidationAttribute
    {
        public PasswordCheckAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var input = (string) value;

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpLowSpecialChar = new Regex(@"[A-Za-z\d@$!%*?&]+");
            var hasMinimum8Chars = new Regex(@".{8,20}");

            var isValidated = hasNumber.IsMatch(input) && hasUpLowSpecialChar.IsMatch(input) &&
                              hasMinimum8Chars.IsMatch(input);

            if (isValidated == true)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Password does not meet minimum strength requirements");
        }
    }
    
    public class PastDateAttribute : ValidationAttribute
    {
        public PastDateAttribute()
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var dt = (DateTime)value;
            if (dt >= DateTime.Now)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Event date cannot be in the past");
        }
    }
    
}

