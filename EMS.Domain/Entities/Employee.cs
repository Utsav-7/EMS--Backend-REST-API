using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EMS_Backend_Project.EMS.Domain.Common.Validation;

namespace EMS_Backend_Project.EMS.Domain.Entities
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "DepartmentId is required.")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateOnly DateOfBirth { get; set; }

        [StringLength(60, ErrorMessage = "Address cannot exceed 60 characters.")]
        public string? Address { get; set; }

        [StringLength(100, ErrorMessage = "Tech Stack cannot exceed 100 characters.")]
        public string? TechStack { get; set; }

        [Required(ErrorMessage = "Join Date is required.")]
        public DateOnly JoinDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [CustomValidation(typeof(Employee), nameof(ValidateJoinAndRelievingDate))]
        public DateOnly? RelievingDate { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<TimeSheet> TimeSheets { get; set; } = new HashSet<TimeSheet>();
        public virtual ICollection<Leave>? Leaves { get; set; } = new HashSet<Leave>();

        // Custom Validation Method
        public static ValidationResult? ValidateJoinAndRelievingDate(DateOnly? relievingDate, ValidationContext context)
        {
            var instance = (Employee)context.ObjectInstance;
            if (relievingDate.HasValue && instance.JoinDate >= relievingDate.Value)
            {
                return new ValidationResult("Relieving Date must be later than Join Date.");
            }
            return ValidationResult.Success;
        }
    }
}