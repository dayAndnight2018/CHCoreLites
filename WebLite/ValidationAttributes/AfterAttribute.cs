using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.ValidationAttributes
{
    public class AfterAttribute : ValidationAttribute
    {
        private DateTime refer = DateTime.Now;
        public AfterAttribute()
        {
        }

        public AfterAttribute(string errorMessage) : base(errorMessage)
        {

        }

        public DateTime ReferTime
        {
            set
            {
                refer = value;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is DateTime) || value == null)
            {
                return new ValidationResult("The type is not DateTime or the value is null.");
            }
            DateTime input = (DateTime)value;
            if (input.CompareTo(refer) <= 0)
            {
                return new ValidationResult("The date is not in a certain range.");
            }
            return ValidationResult.Success;

        }
    }
}
