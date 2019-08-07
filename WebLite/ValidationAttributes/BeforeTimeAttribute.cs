using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebLite.ValidationAttributes
{
    public class BeforeTimeAttribute : ValidationAttribute
    {
        public BeforeTimeAttribute(string otherProperty, long seconds)
        {
            OtherProperty = otherProperty;
            Seconds = seconds;
        }

        public string OtherProperty
        {
            get;
            set;
        }

        public long Seconds
        {
            get;
            set;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // the the other property
            var property = validationContext.ObjectType.GetProperty(OtherProperty);

            if (property == null)
                return new ValidationResult("The property is not find");

            var other = property.GetValue(validationContext.ObjectInstance, null);

            if (!(value is DateTime) || property.PropertyType != typeof(DateTime))
            {
                return new ValidationResult("The value or reference value is not DateTime");
            }

            if (value == null || other == null)
            {
                return new ValidationResult("The value or reference value is null");
            }

            DateTime source = (DateTime)value;
            DateTime refer = (DateTime)other;

            if (source > refer.AddSeconds(0 - Seconds))
            {
                return new ValidationResult($"The interval is at least {Seconds} seconds");
            }

            return ValidationResult.Success;
        }
    }
}
