using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.ValidationAttributes
{
    public class MaxIntegerAttribute : ValidationAttribute
    {
        public MaxIntegerAttribute()
        {
        }

        public MaxIntegerAttribute(string errorMessage) : base(errorMessage)
        {
        }

        public int ReferInteger { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is Int32))
            {
                return new ValidationResult(ErrorMessage??"The number is null or  invalid");
            }
            if ((int)value > ReferInteger)
            {
                return new ValidationResult(ErrorMessage??$"The number is greater than {ReferInteger}");
            }

            return ValidationResult.Success;
        }
    }
}
