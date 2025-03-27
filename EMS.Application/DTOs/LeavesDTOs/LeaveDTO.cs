using EMS_Backend_Project.EMS.Domain.Common.Validators;
using System.ComponentModel.DataAnnotations;

namespace EMS_Backend_Project.EMS.Application.DTOs.LeavesDTOs
{
    public class LeaveDTO
    {
        [Required(ErrorMessage = "Employee ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Employee ID must be a positive number.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [DateGreaterThan(nameof(StartDate), ErrorMessage = "End Date must be after Start Date.")]
        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "Leave Type is required.")]
        [StringLength(50, ErrorMessage = "Leave Type length can't be more than 50 characters.")]
        public string LeaveType { get; set; }

        [StringLength(500, ErrorMessage = "Reason can't be more than 500 characters.")]
        public string? Reason { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status length can't be more than 20 characters.")]
        public string Status { get; set; }
    }
}