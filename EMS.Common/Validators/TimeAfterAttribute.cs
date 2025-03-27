using System.ComponentModel.DataAnnotations;

namespace EMS_Backend_Project.EMS.Domain.Common.Validators
{
    public class TimeAfterAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public TimeAfterAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is TimeSpan currentValue)
            {
                var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
                if (property == null)
                {
                    return new ValidationResult($"Unknown property: {_comparisonProperty}");
                }

                var comparisonValue = (TimeSpan)property.GetValue(validationContext.ObjectInstance);

                if (currentValue <= comparisonValue)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be after {_comparisonProperty}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}