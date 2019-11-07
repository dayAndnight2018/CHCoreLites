using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebLite.ValidationAttributes
{
    public class LengthAttribute : ValidationAttribute
    {
        private int length = 1;
        public LengthAttribute()
        {
        }

        public LengthAttribute(string errorMessage) : base(errorMessage)
        {

        }

        public int ReferLength
        {
            set
            {
                length = value;
            }
            get {
                return length;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is String))
            {
                return new ValidationResult(ErrorMessage??"The value is not String");
            }

            if (value == null || string.IsNullOrWhiteSpace(((String)value).Trim()))
            {
                return new ValidationResult(ErrorMessage??"The value is null or blank");
            }

            if (((string)value).Length != ReferLength)
            {
                return new ValidationResult(ErrorMessage??$"The length of the value is not in a certain range({ReferLength}).");
            }

            return ValidationResult.Success;

        }
    }
}
