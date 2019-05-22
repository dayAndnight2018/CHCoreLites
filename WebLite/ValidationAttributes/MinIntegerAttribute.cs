using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.ValidationAttributes
{
    public class MinIntegerAttribute : ValidationAttribute
    {
        public int ReferInteger { get; set; }

        public MinIntegerAttribute()
        {
        }

        public MinIntegerAttribute(string errorMessage) : base(errorMessage)
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is Int32))
            {
                return new ValidationResult("The number is null or  invalid");
            }
            if ((int)value < ReferInteger)
            {
                return new ValidationResult($"The number is less than {ReferInteger}");
            }

            return ValidationResult.Success;
        }
    }
}
