using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.ValidationAttributes
{
    public class MinDoubleAttribute : ValidationAttribute
    {
        public double ReferDouble { get; set; }

        public MinDoubleAttribute()
        {
        }

        public MinDoubleAttribute(string errorMessage) : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is Double))
            {
                return new ValidationResult(ErrorMessage??"The number is null or  invalid");
            }
            if ((long)value < ReferDouble)
            {
                return new ValidationResult(ErrorMessage??$"The number is less than {ReferDouble}");
            }

            return ValidationResult.Success;
        }
    }
}
