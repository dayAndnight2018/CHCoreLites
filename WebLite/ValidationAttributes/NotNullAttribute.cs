using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.ValidationAttributes
{
    public class NotNullAttribute : ValidationAttribute
    {
        public NotNullAttribute()
        {
        }

        public NotNullAttribute(string errorMessage) : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return new ValidationResult(ErrorMessage??"The value is null");
            }

            return ValidationResult.Success;
        }
    }
}
