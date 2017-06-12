using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SKNManager.Utils.Validation
{
    public class IsDateAfterAttribute : ValidationAttribute
    {
        private readonly string testedPropertyName;
        private readonly bool allowEqualDates;

        public IsDateAfterAttribute(string testedPropertyName, bool allowEqualDates = true)
        {
            this.testedPropertyName = testedPropertyName;
            this.allowEqualDates = allowEqualDates;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.testedPropertyName);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult(string.Format("nieznane pole {0}", this.testedPropertyName));
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

            if (value == null || !(value is DateTime))
            {
                return ValidationResult.Success;
            }

            if (propertyTestedValue == null || !(propertyTestedValue is DateTime))
            {
                return ValidationResult.Success;
            }

            // Compare values
            if (DateTime.Compare((DateTime)value, (DateTime)propertyTestedValue) >= 0)
            {
                if (this.allowEqualDates && DateTime.Compare((DateTime)value, (DateTime)propertyTestedValue) == 0)
                {
                    return ValidationResult.Success;
                }
                else if (DateTime.Compare((DateTime)value, (DateTime)propertyTestedValue) > 0)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
