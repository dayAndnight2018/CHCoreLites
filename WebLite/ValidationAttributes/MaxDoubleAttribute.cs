using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.ValidationAttributes
{
    public class MaxDoubleAttribute : ValidationAttribute
    {
        public double  ReferDouble { get; set; }

        public MaxDoubleAttribute()
        {
        }

        public MaxDoubleAttribute(string errorMessage) : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is Double))
            {
                return new ValidationResult("The number is null or  invalid");
            }
            if ((long)value > ReferDouble)
            {
                return new ValidationResult($"The number is great than {ReferDouble}");
            }

            return ValidationResult.Success;
        }
    }
}
