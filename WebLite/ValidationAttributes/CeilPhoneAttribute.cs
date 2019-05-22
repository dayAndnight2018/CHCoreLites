using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebLite.ValidationAttributes
{
    public class CeilPhoneAttribute : ValidationAttribute
    {
        public CeilPhoneAttribute()
        {
        }

        public CeilPhoneAttribute(string errorMessage) : base(errorMessage)
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ( (value == null) || !(value is String))
            {
                return new ValidationResult("The ceilphone number is null");
            }

            bool result = Regex.IsMatch((String)value, @"^[1]+[3,5,8,4,7,9]+\d{9}");
            if (!result)
            {
                return new ValidationResult("The ceilphone number is invalid");
            }

            return ValidationResult.Success;
        }
    }
}
