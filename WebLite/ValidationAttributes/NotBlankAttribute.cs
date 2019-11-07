using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.ValidationAttributes
{
    public class NotBlankAttribute : ValidationAttribute
    {
        public NotBlankAttribute()
        {
        }

        public NotBlankAttribute(string errorMessage) : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is String))
            {
                return new ValidationResult(ErrorMessage??"The value is not String");
            }
            if (value == null || string.IsNullOrWhiteSpace(((String)value).Trim()))
            {
                return new ValidationResult(ErrorMessage ?? "The value is null or blank");
            }

            return ValidationResult.Success;
        }
    }
}
