using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.ValidationAttributes
{
    public class MinFloatAttribute : ValidationAttribute
    {
        public float ReferFloat { get; set; }

        public MinFloatAttribute()
        {
        }

        public MinFloatAttribute(string errorMessage) : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is float))
            {
                return new ValidationResult("The number is null or  invalid");
            }
            if ((long)value < ReferFloat)
            {
                return new ValidationResult($"The number is less than {ReferFloat}");
            }

            return ValidationResult.Success;
        }
    }
}
