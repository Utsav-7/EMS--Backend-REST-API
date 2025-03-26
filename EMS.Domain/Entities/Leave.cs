using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS_Backend_Project.EMS.Domain.Entities
{
    public class Leave
    {
        [Key]
        public int LeaveId { get; set; }

        [Required(ErrorMessage = "Employee ID is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [CustomValidation(typeof(Leave), nameof(ValidateStartAndEndDate))]
        public DateOnly EndDate { get; set; }

        public int TotalDays
        {
            get
            {
                return (EndDate < StartDate) ? 0 : (EndDate.ToDateTime(TimeOnly.MinValue) - StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1;
            }
            set { }
        }

        [Required(ErrorMessage = "Leave Type is required.")]
        [StringLength(50, ErrorMessage = "Leave Type cannot exceed 50 characters.")]
        public string LeaveType { get; set; }

        [StringLength(250, ErrorMessage = "Reason cannot exceed 250 characters.")]
        public string? Reason { get; set; }

        [Required(ErrorMessage = "Leave Status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string Status { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public virtual Employee Employee { get; set; }

        // Custom Validation for StartDate < EndDate
        public static ValidationResult? ValidateStartAndEndDate(DateOnly endDate, ValidationContext context)
        {
            var instance = (Leave)context.ObjectInstance;
            if (endDate < instance.StartDate)
            {
                return new ValidationResult("End Date must be later than or equal to Start Date.");
            }
            return ValidationResult.Success;
        }
    }
}