using System.ComponentModel.DataAnnotations;

namespace EMS_Backend_Project.EMS.Domain.Common.Validation
{
    public class CustomStringLengthAttribute : StringLengthAttribute
    {
        public CustomStringLengthAttribute(int maximumLength) : base(maximumLength)
        {
            MinimumLength = 2; // Set minimum length globally
            ErrorMessage = "The field {0} must be between {2} and {1} characters.";
        }
    }
}
