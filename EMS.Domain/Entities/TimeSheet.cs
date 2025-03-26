using System;
using System.ComponentModel.DataAnnotations;
using EMS_Backend_Project.EMS.Domain.Common.Validation;

namespace EMS_Backend_Project.EMS.Domain.Entities
{
    public class TimeSheet
    {
        [Key]
        public int TimeSheetId { get; set; }

        [Required(ErrorMessage = "Employee ID is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Work Date is required.")]
        public DateOnly WorkDate { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required.")]
        [CustomValidation(typeof(TimeSheet), nameof(ValidateStartAndEndTime))]
        public TimeSpan EndTime { get; set; }

        [Range(typeof(TimeSpan), "00:00:00", "12:00:00", ErrorMessage = "Break time should be between 0 and 12 hours.")]
        public TimeSpan BreakTime { get; set; } = TimeSpan.Zero;

        [CustomStringLength(50)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Computed Property (Not Mapped in EF)
        public TimeSpan TotalHours
        {
            get
            {
                var workDuration = EndTime - StartTime;
                return workDuration > BreakTime ? workDuration - BreakTime : TimeSpan.Zero;
            }
            set { }
        }

        // Navigation Property
        public virtual Employee Employee { get; set; }

        // Custom Validation for StartTime < EndTime
        public static ValidationResult? ValidateStartAndEndTime(TimeSpan endTime, ValidationContext context)
        {
            var instance = (TimeSheet)context.ObjectInstance;
            if (endTime <= instance.StartTime)
            {
                return new ValidationResult("End Time must be greater than Start Time.");
            }
            return ValidationResult.Success;
        }
    }
}
