using System.ComponentModel.DataAnnotations;

namespace EMS_Backend_Project.EMS.Domain.Common.Validators
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateOnly currentValue)
            {
                var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
                if (property == null)
                {
                    return new ValidationResult($"Unknown property: {_comparisonProperty}");
                }

                var comparisonValue = (DateOnly)property.GetValue(validationContext.ObjectInstance);

                if (currentValue <= comparisonValue)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be greater than {_comparisonProperty}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}