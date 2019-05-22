using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.ValidationAttributes
{
    public class BeforeAttribute : ValidationAttribute
    {
        private DateTime refer = DateTime.Now;
        public BeforeAttribute()
        {
        }

        public BeforeAttribute(string errorMessage) : base(errorMessage)
        {

        }

        public string ReferTime
        {
            get
            {
                return refer.ToString("yyyy/MM/dd HH:mm:ss");
            }
            set
            {
                DateTime dateTime;
                if (DateTime.TryParse(value, out dateTime))
                {
                    refer = dateTime;
                }
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is DateTime) || value == null)
            {
                return new ValidationResult("The type is not DateTime or the value is null.");
            }
            DateTime input = (DateTime)value;
            if (input.CompareTo(refer) >= 0)
            {
                return new ValidationResult("The date is not in a certain range.");
            }
            return ValidationResult.Success;
        }
    }
}
