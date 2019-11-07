using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.ValidationAttributes
{
    public class MinLongIntegerAttribute : ValidationAttribute
    {
        public long ReferLongInteger { get; set; }

        public MinLongIntegerAttribute()
        {
        }

        public MinLongIntegerAttribute(string errorMessage) : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is Int64))
            {
                return new ValidationResult(ErrorMessage??"The number is null or  invalid");
            }
            if ((long)value < ReferLongInteger)
            {
                return new ValidationResult(ErrorMessage??$"The number is less than {ReferLongInteger}");
            }

            return ValidationResult.Success;
        }
    }
}
